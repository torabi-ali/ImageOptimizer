using System.IO;
using System.Windows;
using System.Windows.Controls;
using static System.Environment;

namespace ImageOptimizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Initialization();
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        public void Initialization()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), GotFocusEvent, new RoutedEventHandler(TextBox_GotFocus));

            if (string.IsNullOrEmpty(Properties.Settings.Default.DefaultPath))
            {
                Properties.Settings.Default.DefaultPath = $"{GetFolderPath(SpecialFolder.Desktop)}\\Image Optimizer\\";
                Properties.Settings.Default.Save();
            }

            bool exists = Directory.Exists(Properties.Settings.Default.DefaultPath);
            if (!exists)
            {
                Directory.CreateDirectory(Properties.Settings.Default.DefaultPath);
            }
        }
    }
}
