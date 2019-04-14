using ImageOptimizer.Helpers;
using ImageOptimizer.Model;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;

namespace ImageOptimizer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private static readonly BackgroundWorker bw = new BackgroundWorker();
        private Optimize Optimize;

        private CompressImage _compressImage;
        public CompressImage CompressImage
        {
            get => _compressImage;
            set { _compressImage = value; RaisePropertyChanged(nameof(_compressImage)); }
        }
        #endregion

        #region Commands
        public RelayCommand OptimizeCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }
        public RelayCommand SettingCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            #region Fields
            bw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
            #endregion

            #region Commands
            OptimizeCommand = new RelayCommand(OptimizeImage);
            AboutCommand = new RelayCommand(About);
            SettingCommand = new RelayCommand(Setting);
            CancelCommand = new RelayCommand(Cancel);
            #endregion
        }

        private void OptimizeImage(object parameter)
        {
            CurrentProgress = 0;
            RaisePropertyChanged("CompressImage");

            var fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Select your image",
                Filter = "Image Files | *.jpg; *.png; *.gif; *.tif; *.bmp",
                InitialDirectory = Properties.Settings.Default.DefaultPath,
            };
            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                CompressImage = new CompressImage(fileDialog.FileName);
                bw.RunWorkerAsync();
            }
        }

        private void About(object parameter)
        {

        }

        private void Setting(object parameter)
        {
            var setting = new SettingWindow();
            setting.Show();
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Optimize = new Optimize(CompressImage);

            var upload = Optimize.VgyUpload();
            if (upload)
            {
                CurrentProgress = 50;
                RaisePropertyChanged("CompressImage");
                var optimized = Optimize.ResmushOptimize();
                if (optimized)
                {
                    CurrentProgress = 75;
                    Optimize.ResmushDownload();
                    //VgyDelete(); //Not implemented yet
                    RaisePropertyChanged("CompressImage");
                }
            }
        }

        public void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CurrentProgress = 100;
            MessageBox.Show("Image is Optimized.", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
