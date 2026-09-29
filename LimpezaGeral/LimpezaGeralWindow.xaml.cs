using System.Windows;

namespace BimBlaster.LimpezaGeral
{
    public partial class LimpezaGeralWindow : Window
    {
        public bool Cancelled { get; private set; } = true;
        public CleanupOptions Options { get; private set; } = new CleanupOptions();

        public LimpezaGeralWindow(AppLanguage lang)
        {
            // O dicionário de estilos precisa estar em Resources antes de InitializeComponent().
            Resources.MergedDictionaries.Add(StyleLoader.LoadBimBlasterStyles());
            InitializeComponent();

            string T(string key) => LocalizedStrings.Get(lang, key);
            Title = T("WindowTitle");
            HeaderText.Text = T("Header");
            LblSheets.Text = T("Sheets");
            LblViews.Text = T("Views");
            LblSchedules.Text = T("Schedules");
            LblLinePatterns.Text = T("LinePatterns");
            LblExtra.Text = T("Extra");
            ChkDwg.Content = T("ChkDwg");
            ChkLinks.Content = T("ChkLinks");
            ChkCollections.Content = T("ChkCollections");
            ChkGroups.Content = T("ChkGroups");
            ChkFamilies.Content = T("ChkFamilies");
            BtnCancel.Content = T("Cancel");
            BtnRun.Content = T("Run");
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            Options = new CleanupOptions
            {
                KeepSheetText = TxtSheets.Text,
                KeepViewText = TxtViews.Text,
                KeepScheduleText = TxtSchedules.Text,
                LinePatternPrefix = TxtLinePatterns.Text,
                DeleteDwgs = ChkDwg.IsChecked == true,
                DeleteLinks = ChkLinks.IsChecked == true,
                DeleteSheetCollections = ChkCollections.IsChecked == true,
                DeleteGroupTypes = ChkGroups.IsChecked == true,
                DeleteFamilies = ChkFamilies.IsChecked == true
            };
            Cancelled = false;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            Close();
        }
    }
}
