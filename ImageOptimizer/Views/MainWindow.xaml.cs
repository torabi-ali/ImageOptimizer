using System.Windows;
using System.Windows.Controls;

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
        }

        private void Url_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(txtUrl.Text);
            MessageBox.Show("Text Copied to Clipboard.");
        }
    }
}
