using System.Windows;
using System.Windows.Controls;

namespace ImageOptimizer
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
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
        }
    }
}
