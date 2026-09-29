using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BimBlaster.LimpezaGeral
{
    public static class IconLoader
    {
        public static BitmapImage Load(string relativePath)
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(Path.Combine(baseDir, relativePath)));
        }
    }

    public static class StyleLoader
    {
        public static ResourceDictionary LoadBimBlasterStyles()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(baseDir, "BimBlasterStyles.xaml");
            return new ResourceDictionary { Source = new Uri(path, UriKind.Absolute) };
        }
    }
}
