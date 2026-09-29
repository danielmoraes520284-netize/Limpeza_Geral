using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;

namespace BimBlaster.LimpezaGeral
{
    public class LimpezaGeralApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            const string panelName = "BIM BLASTER";
            var panel = application.GetRibbonPanels().FirstOrDefault(p => p.Name == panelName)
                        ?? application.CreateRibbonPanel(panelName);

            var lang = application.ControlledApplication.Language;
            var data = new PushButtonData(
                "LimpezaGeral",
                LocalizedStrings.Get(lang, "ButtonText"),
                Assembly.GetExecutingAssembly().Location,
                "BimBlaster.LimpezaGeral.LimpezaGeralCommand")
            {
                ToolTip = LocalizedStrings.Get(lang, "ButtonTooltip"),
                LargeImage = IconLoader.Load("icon32.png"),
                Image = IconLoader.Load("icon16.png")
            };

            panel.AddItem(data);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application) => Result.Succeeded;
    }
}
