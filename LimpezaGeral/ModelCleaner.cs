using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace BimBlaster.LimpezaGeral
{
    public class CleanupOptions
    {
        public string KeepSheetText { get; set; } = "";
        public string KeepViewText { get; set; } = "";
        public string KeepScheduleText { get; set; } = "";
        public string LinePatternPrefix { get; set; } = "";
        public bool DeleteDwgs { get; set; }
        public bool DeleteLinks { get; set; }
        public bool DeleteSheetCollections { get; set; }
        public bool DeleteGroupTypes { get; set; }
        public bool DeleteFamilies { get; set; }
    }

    public class ModelCleaner
    {
        private readonly Document _doc;
        private readonly ElementId _activeViewId;
        private readonly AppLanguage _lang;

        public List<string> Logs { get; } = new List<string>();

        public ModelCleaner(Document doc, ElementId activeViewId, AppLanguage lang)
        {
            _doc = doc;
            _activeViewId = activeViewId;
            _lang = lang;
        }

        private string L(string key, params object[] args) =>
            string.Format(LocalizedStrings.Get(_lang, key), args);

        // Deleta em lote; se falhar, tenta um a um.
        private int DeleteSafe(ICollection<ElementId> ids)
        {
            if (ids == null || ids.Count == 0) return 0;
            try
            {
                return _doc.Delete(ids)?.Count ?? 0;
            }
            catch (Exception)
            {
                int count = 0;
                foreach (var id in ids)
                {
                    try
                    {
                        if (_doc.GetElement(id) == null) continue;
                        count += _doc.Delete(id)?.Count ?? 0;
                    }
                    catch (Exception) { }
                }
                return count;
            }
        }

        private int InTransaction(string name, ICollection<ElementId> ids)
        {
            if (ids == null || ids.Count == 0) return 0;
            using (var t = new Transaction(_doc, name))
            {
                t.Start();
                int removed = DeleteSafe(ids);
                t.Commit();
                return removed;
            }
        }

        public void Run(CleanupOptions o)
        {
            using (var group = new TransactionGroup(_doc, "Limpeza Geral"))
            {
                group.Start();

                Step1Sheets(o);
                Step2Views(o);
                Step3Schedules(o);
                Step4ViewTemplates();
                Step5ViewFilters();
                Step6LinePatterns(o);
                Step7Dwgs(o);
                Step8Links(o);
                Step9SheetCollections(o);
                Step10GroupTypes(o);
                Step11Families(o);

                group.Assimilate();
            }
        }

        private void Step1Sheets(CleanupOptions o)
        {
            var keep = (o.KeepSheetText ?? "").Trim().ToLowerInvariant();
            var ids = new FilteredElementCollector(_doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>()
                .Where(s => !s.Name.ToLowerInvariant().Contains(keep) &&
                            !s.SheetNumber.ToLowerInvariant().Contains(keep))
                .Select(s => s.Id).ToList();
            Logs.Add(L("Log1", InTransaction("1. Folhas", ids)));
        }

        private void Step2Views(CleanupOptions o)
        {
            var keep = (o.KeepViewText ?? "").Trim().ToLowerInvariant();
            var toDelete = new List<ElementId>();
            int kept = 0;

            foreach (var view in new FilteredElementCollector(_doc).OfClass(typeof(View))
                         .WhereElementIsNotElementType().Cast<View>())
            {
                // Folhas e tabelas têm etapas próprias (1 e 3), que respeitam os filtros de "manter".
                if (view.IsTemplate || view.Id == _activeViewId || !view.CanBePrinted ||
                    view is ViewSheet || view is ViewSchedule)
                    continue;

                if (keep.Length > 0 && view.Name.ToLowerInvariant().Contains(keep)) kept++;
                else toDelete.Add(view.Id);
            }

            Logs.Add(L("Log2", InTransaction("2. Vistas", toDelete), kept));
        }

        private void Step3Schedules(CleanupOptions o)
        {
            var keep = (o.KeepScheduleText ?? "").Trim().ToLowerInvariant();
            var toDelete = new List<ElementId>();
            int kept = 0;

            foreach (var s in new FilteredElementCollector(_doc).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>())
            {
                if (s.IsTemplate) continue;
                var name = s.Name.ToLowerInvariant();
                if (name.Contains("revis") || name.Contains("revision") ||
                    (keep.Length > 0 && name.Contains(keep)))
                {
                    kept++;
                    continue;
                }
                toDelete.Add(s.Id);
            }

            Logs.Add(L("Log3", InTransaction("3. Tabelas", toDelete), kept));
        }

        private void Step4ViewTemplates()
        {
            var views = new FilteredElementCollector(_doc).OfClass(typeof(View)).Cast<View>().ToList();
            var inUse = new HashSet<ElementId>();
            foreach (var v in views)
                if (!v.IsTemplate && v.ViewTemplateId != ElementId.InvalidElementId)
                    inUse.Add(v.ViewTemplateId);

            var ids = views.Where(v => v.IsTemplate && !inUse.Contains(v.Id)).Select(v => v.Id).ToList();
            Logs.Add(L("Log4", InTransaction("4. Modelos de vista", ids)));
        }

        private void Step5ViewFilters()
        {
            var inUse = new HashSet<ElementId>();
            foreach (var v in new FilteredElementCollector(_doc).OfClass(typeof(View)).Cast<View>())
            {
                try
                {
                    foreach (var id in v.GetFilters()) inUse.Add(id);
                }
                catch (Exception) { }
            }

            var ids = new FilteredElementCollector(_doc).OfClass(typeof(ParameterFilterElement))
                .Select(f => f.Id).Where(id => !inUse.Contains(id)).ToList();
            Logs.Add(L("Log5", InTransaction("5. Filtros de vista", ids)));
        }

        private void Step6LinePatterns(CleanupOptions o)
        {
            var prefix = (o.LinePatternPrefix ?? "").Trim().ToLowerInvariant();
            var ids = prefix.Length == 0
                ? new List<ElementId>()
                : new FilteredElementCollector(_doc).OfClass(typeof(LinePatternElement))
                    .Where(e => e.Name.ToLowerInvariant().StartsWith(prefix, StringComparison.Ordinal))
                    .Select(e => e.Id).ToList();
            Logs.Add(L("Log6", InTransaction("6. Padrões de linha", ids)));
        }

        private void Step7Dwgs(CleanupOptions o)
        {
            if (!o.DeleteDwgs) { Logs.Add(L("Log7Kept")); return; }

            var ids = new List<ElementId>();
            foreach (var inst in new FilteredElementCollector(_doc).OfClass(typeof(ImportInstance)).Cast<ImportInstance>())
            {
                if (inst.IsLinked) continue;
                var symbol = _doc.GetElement(inst.GetTypeId());
                var p = symbol?.get_Parameter(BuiltInParameter.SYMBOL_NAME_PARAM);
                if (p != null && p.HasValue)
                {
                    var name = p.AsString();
                    if (name != null && name.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase))
                        ids.Add(inst.Id);
                }
            }
            Logs.Add(L("Log7", InTransaction("7. DWGs importados", ids)));
        }

        private void Step8Links(CleanupOptions o)
        {
            if (!o.DeleteLinks) { Logs.Add(L("Log8Kept")); return; }

            var ids = new List<ElementId>();
            ids.AddRange(new FilteredElementCollector(_doc).OfClass(typeof(RevitLinkType)).Select(e => e.Id));
            ids.AddRange(new FilteredElementCollector(_doc).OfClass(typeof(CADLinkType)).Select(e => e.Id));
            ids.AddRange(new FilteredElementCollector(_doc).OfClass(typeof(ImageType)).Select(e => e.Id));
            Logs.Add(L("Log8", InTransaction("8. Vínculos", ids)));
        }

        private void Step9SheetCollections(CleanupOptions o)
        {
            if (!o.DeleteSheetCollections) { Logs.Add(L("Log9Kept")); return; }

            // SheetCollection só existe nas versões mais novas da API; resolvido por reflexão
            // para que o mesmo código compile no Revit 2024.
            var type = typeof(Document).Assembly.GetType("Autodesk.Revit.DB.SheetCollection");
            if (type == null) { Logs.Add(L("Log9Unsupported")); return; }

            var ids = new FilteredElementCollector(_doc).OfClass(type).Select(e => e.Id).ToList();
            Logs.Add(L("Log9", InTransaction("9. Coleções de folhas", ids)));
        }

        private void Step10GroupTypes(CleanupOptions o)
        {
            if (!o.DeleteGroupTypes) { Logs.Add(L("Log10Kept")); return; }

            var inUse = new HashSet<ElementId>(
                new FilteredElementCollector(_doc).OfClass(typeof(Group)).Select(g => g.GetTypeId()));
            var ids = new FilteredElementCollector(_doc).OfClass(typeof(GroupType))
                .Select(t => t.Id).Where(id => !inUse.Contains(id)).ToList();
            Logs.Add(L("Log10", InTransaction("10. Tipos de grupos", ids)));
        }

        private void Step11Families(CleanupOptions o)
        {
            if (!o.DeleteFamilies) { Logs.Add(L("Log11Kept")); return; }

            var ids = new List<ElementId>();
            foreach (var fam in new FilteredElementCollector(_doc).OfClass(typeof(Family)).Cast<Family>())
            {
                var symbolIds = fam.GetFamilySymbolIds();
                if (symbolIds == null || symbolIds.Count == 0) continue;

                bool inUse = false;
                foreach (var symId in symbolIds)
                {
                    var filter = new FamilyInstanceFilter(_doc, symId);
                    if (new FilteredElementCollector(_doc).WherePasses(filter).GetElementCount() > 0)
                    {
                        inUse = true;
                        break;
                    }
                }
                if (!inUse) ids.Add(fam.Id);
            }
            Logs.Add(L("Log11", InTransaction("11. Famílias não utilizadas", ids)));
        }
    }
}
