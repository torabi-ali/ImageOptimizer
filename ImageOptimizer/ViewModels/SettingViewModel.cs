using ImageOptimizer.Helpers;
using System.Windows;

namespace ImageOptimizer.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        #region Fields
        private int _qaulity;
        public int Quality
        {
            get => _qaulity;
            set { _qaulity = value; RaisePropertyChanged(nameof(_qaulity)); }
        }
        #endregion

        #region Commands
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion


        public SettingViewModel()
        {
            #region Fields
            Quality = Properties.Settings.Default.Quality;
            #endregion

            #region Commands
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);
            #endregion
        }

        private void Apply(object parameter)
        {
            Properties.Settings.Default.Quality = Quality;
            Properties.Settings.Default.Save();
            MessageBox.Show("Setting have been saved", "Done", MessageBoxButton.OK, MessageBoxImage.Information);

            Cancel(parameter);
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }
    }
}
