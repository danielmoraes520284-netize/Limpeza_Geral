using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;

namespace BimBlaster.LimpezaGeral
{
    public enum AppLanguage { Portuguese, English, Spanish }

    public static class LanguageResolver
    {
        public static AppLanguage Resolve(LanguageType revitLanguage)
        {
            switch (revitLanguage)
            {
                case LanguageType.Spanish: return AppLanguage.Spanish;
                case LanguageType.English_USA: return AppLanguage.English;
                default: return AppLanguage.Portuguese;
            }
        }
    }

    public static class LocalizedStrings
    {
        private static readonly Dictionary<AppLanguage, Dictionary<string, string>> Strings =
            new Dictionary<AppLanguage, Dictionary<string, string>>
            {
                [AppLanguage.Portuguese] = new Dictionary<string, string>
                {
                    ["ButtonText"] = "Limpeza Geral",
                    ["ButtonTooltip"] = "Limpeza geral do modelo para envio: remove vistas, tabelas, modelos e filtros de vista, padrões de linha, DWGs, vínculos, folhas, coleções de folhas, grupos e famílias não usadas.",
                    ["WindowTitle"] = "Limpeza Geral do Modelo",
                    ["Header"] = "Limpeza Geral",
                    ["Sheets"] = "1. Folhas (NÃO deletar - texto/número):",
                    ["Views"] = "2. Vistas (MANTER - trecho de texto):",
                    ["Schedules"] = "3. Tabelas (MANTER - trecho de texto):",
                    ["LinePatterns"] = "6. Padrões de Linha (deletar prefixo comum):",
                    ["Extra"] = "Opções adicionais de exclusão",
                    ["ChkDwg"] = "7. Deletar todos os DWGs importados (não vinculados)",
                    ["ChkLinks"] = "8. Remover todos os vínculos (Revit, CAD e imagens)",
                    ["ChkCollections"] = "9. Deletar Coleções de Folhas (Revit 2025+)",
                    ["ChkGroups"] = "10. Deletar tipos de grupos não utilizados",
                    ["ChkFamilies"] = "11. Deletar famílias carregáveis não utilizadas",
                    ["Cancel"] = "Cancelar",
                    ["Run"] = "Executar",
                    ["Confirm"] = "Esta operação deleta elementos do modelo ativo. Deseja continuar?",
                    ["ConfirmTitle"] = "Confirmar limpeza",
                    ["SummaryTitle"] = "Sumário Geral da Limpeza",
                    ["ErrorTitle"] = "Erro na Execução",
                    ["ErrorMsg"] = "Ocorreu um erro durante a execução:",
                    ["Log1"] = "1. Folhas: {0} elemento(s) removido(s).",
                    ["Log2"] = "2. Vistas: {0} elemento(s) removido(s) | {1} mantida(s).",
                    ["Log3"] = "3. Tabelas: {0} elemento(s) removido(s) | {1} mantida(s).",
                    ["Log4"] = "4. Modelos de vista não utilizados removidos: {0}",
                    ["Log5"] = "5. Filtros de vista não utilizados removidos: {0}",
                    ["Log6"] = "6. Padrões de linha removidos: {0}",
                    ["Log7"] = "7. DWGs importados removidos: {0}",
                    ["Log7Kept"] = "7. DWGs importados: mantidos pelo usuário.",
                    ["Log8"] = "8. Vínculos removidos: {0}",
                    ["Log8Kept"] = "8. Vínculos: mantidos pelo usuário.",
                    ["Log9"] = "9. Coleções de folhas removidas: {0}",
                    ["Log9Kept"] = "9. Coleções de folhas: mantidas pelo usuário.",
                    ["Log9Unsupported"] = "9. Coleções de folhas: recurso não suportado nesta versão do Revit.",
                    ["Log10"] = "10. Tipos de grupos não utilizados removidos: {0}",
                    ["Log10Kept"] = "10. Tipos de grupos: mantidos pelo usuário.",
                    ["Log11"] = "11. Famílias não utilizadas removidas: {0}",
                    ["Log11Kept"] = "11. Famílias: mantidas pelo usuário.",
                },
                [AppLanguage.English] = new Dictionary<string, string>
                {
                    ["ButtonText"] = "Limpeza Geral",
                    ["ButtonTooltip"] = "General model cleanup for delivery: removes views, schedules, view templates, view filters, line patterns, DWGs, links, sheets, sheet collections, groups and unused families.",
                    ["WindowTitle"] = "General Model Cleanup",
                    ["Header"] = "General Cleanup",
                    ["Sheets"] = "1. Sheets (do NOT delete - text/number):",
                    ["Views"] = "2. Views (KEEP - text fragment):",
                    ["Schedules"] = "3. Schedules (KEEP - text fragment):",
                    ["LinePatterns"] = "6. Line Patterns (delete common prefix):",
                    ["Extra"] = "Additional deletion options",
                    ["ChkDwg"] = "7. Delete all imported DWGs (not linked)",
                    ["ChkLinks"] = "8. Remove all links (Revit, CAD and images)",
                    ["ChkCollections"] = "9. Delete Sheet Collections (Revit 2025+)",
                    ["ChkGroups"] = "10. Delete unused group types",
                    ["ChkFamilies"] = "11. Delete unused loadable families",
                    ["Cancel"] = "Cancel",
                    ["Run"] = "Run",
                    ["Confirm"] = "This operation deletes elements from the active model. Continue?",
                    ["ConfirmTitle"] = "Confirm cleanup",
                    ["SummaryTitle"] = "Cleanup Summary",
                    ["ErrorTitle"] = "Execution Error",
                    ["ErrorMsg"] = "An error occurred during execution:",
                    ["Log1"] = "1. Sheets: {0} element(s) removed.",
                    ["Log2"] = "2. Views: {0} element(s) removed | {1} kept.",
                    ["Log3"] = "3. Schedules: {0} element(s) removed | {1} kept.",
                    ["Log4"] = "4. Unused view templates removed: {0}",
                    ["Log5"] = "5. Unused view filters removed: {0}",
                    ["Log6"] = "6. Line patterns removed: {0}",
                    ["Log7"] = "7. Imported DWGs removed: {0}",
                    ["Log7Kept"] = "7. Imported DWGs: kept by user.",
                    ["Log8"] = "8. Links removed: {0}",
                    ["Log8Kept"] = "8. Links: kept by user.",
                    ["Log9"] = "9. Sheet collections removed: {0}",
                    ["Log9Kept"] = "9. Sheet collections: kept by user.",
                    ["Log9Unsupported"] = "9. Sheet collections: feature not supported in this Revit version.",
                    ["Log10"] = "10. Unused group types removed: {0}",
                    ["Log10Kept"] = "10. Group types: kept by user.",
                    ["Log11"] = "11. Unused families removed: {0}",
                    ["Log11Kept"] = "11. Families: kept by user.",
                },
                [AppLanguage.Spanish] = new Dictionary<string, string>
                {
                    ["ButtonText"] = "Limpeza Geral",
                    ["ButtonTooltip"] = "Limpieza general del modelo para envío: elimina vistas, tablas, plantillas y filtros de vista, patrones de línea, DWG, vínculos, hojas, colecciones de hojas, grupos y familias no utilizadas.",
                    ["WindowTitle"] = "Limpieza General del Modelo",
                    ["Header"] = "Limpieza General",
                    ["Sheets"] = "1. Hojas (NO eliminar - texto/número):",
                    ["Views"] = "2. Vistas (MANTENER - fragmento de texto):",
                    ["Schedules"] = "3. Tablas (MANTENER - fragmento de texto):",
                    ["LinePatterns"] = "6. Patrones de línea (eliminar prefijo común):",
                    ["Extra"] = "Opciones adicionales de eliminación",
                    ["ChkDwg"] = "7. Eliminar todos los DWG importados (no vinculados)",
                    ["ChkLinks"] = "8. Quitar todos los vínculos (Revit, CAD e imágenes)",
                    ["ChkCollections"] = "9. Eliminar colecciones de hojas (Revit 2025+)",
                    ["ChkGroups"] = "10. Eliminar tipos de grupo no utilizados",
                    ["ChkFamilies"] = "11. Eliminar familias cargables no utilizadas",
                    ["Cancel"] = "Cancelar",
                    ["Run"] = "Ejecutar",
                    ["Confirm"] = "Esta operación elimina elementos del modelo activo. ¿Continuar?",
                    ["ConfirmTitle"] = "Confirmar limpieza",
                    ["SummaryTitle"] = "Resumen de la Limpieza",
                    ["ErrorTitle"] = "Error de Ejecución",
                    ["ErrorMsg"] = "Se produjo un error durante la ejecución:",
                    ["Log1"] = "1. Hojas: {0} elemento(s) eliminado(s).",
                    ["Log2"] = "2. Vistas: {0} elemento(s) eliminado(s) | {1} mantenida(s).",
                    ["Log3"] = "3. Tablas: {0} elemento(s) eliminado(s) | {1} mantenida(s).",
                    ["Log4"] = "4. Plantillas de vista no utilizadas eliminadas: {0}",
                    ["Log5"] = "5. Filtros de vista no utilizados eliminados: {0}",
                    ["Log6"] = "6. Patrones de línea eliminados: {0}",
                    ["Log7"] = "7. DWG importados eliminados: {0}",
                    ["Log7Kept"] = "7. DWG importados: mantenidos por el usuario.",
                    ["Log8"] = "8. Vínculos eliminados: {0}",
                    ["Log8Kept"] = "8. Vínculos: mantenidos por el usuario.",
                    ["Log9"] = "9. Colecciones de hojas eliminadas: {0}",
                    ["Log9Kept"] = "9. Colecciones de hojas: mantenidas por el usuario.",
                    ["Log9Unsupported"] = "9. Colecciones de hojas: función no compatible con esta versión de Revit.",
                    ["Log10"] = "10. Tipos de grupo no utilizados eliminados: {0}",
                    ["Log10Kept"] = "10. Tipos de grupo: mantenidos por el usuario.",
                    ["Log11"] = "11. Familias no utilizadas eliminadas: {0}",
                    ["Log11Kept"] = "11. Familias: mantenidas por el usuario.",
                },
            };

        public static string Get(LanguageType revitLanguage, string key) =>
            Get(LanguageResolver.Resolve(revitLanguage), key);

        public static string Get(AppLanguage language, string key)
        {
            if (Strings[language].TryGetValue(key, out var value)) return value;
            return Strings[AppLanguage.Portuguese][key];
        }
    }
}
