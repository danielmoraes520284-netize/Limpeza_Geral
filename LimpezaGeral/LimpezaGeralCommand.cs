using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimBlaster.LimpezaGeral
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class LimpezaGeralCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            if (uiDoc == null) return Result.Cancelled;

            var lang = LanguageResolver.Resolve(uiApp.Application.Language);
            var window = new LimpezaGeralWindow(lang);
            window.ShowDialog();
            if (window.Cancelled) return Result.Cancelled;

            var cleaner = new ModelCleaner(uiDoc.Document, uiDoc.ActiveView?.Id ?? ElementId.InvalidElementId, lang);
            try
            {
                cleaner.Run(window.Options);
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show(LocalizedStrings.Get(lang, "ErrorTitle"),
                    LocalizedStrings.Get(lang, "ErrorMsg") + "\n" + ex.Message);
                return Result.Failed;
            }

            TaskDialog.Show(LocalizedStrings.Get(lang, "SummaryTitle"), string.Join("\n", cleaner.Logs));
            return Result.Succeeded;
        }
    }
}
