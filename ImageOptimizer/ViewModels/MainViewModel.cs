using ImageOptimizer.Helpers;
using ImageOptimizer.Model;
using ImageOptimizer.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace ImageOptimizer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private static readonly BackgroundWorker bw = new BackgroundWorker();
        private Optimize Optimize;
        private FunctionResult FunctionResult;

        private CompressImage _compressImage;
        private readonly List<CompressImage> CompressImageCollection = new List<CompressImage>();
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
            FunctionResult = 0;
            RaisePropertyChanged("CompressImage");

            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Select your image",
                Filter = "Image Files | *.jpg; *.png; *.gif; *.tif; *.bmp",
            };
            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                foreach (var file in fileDialog.FileNames)
                {
                    var compressImage = new CompressImage(file);
                    CompressImageCollection.Add(compressImage);
                }
                bw.RunWorkerAsync();
            }
        }

        private void About(object parameter)
        {
            var myProcess = new Process();

            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "https://github.com/torabi-ali/ImageOptimizer";
                myProcess.Start();
            }
            catch (Exception ex)
            {
                ex.Log();
            }
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
            var progressChunk = 100.0 / CompressImageCollection.Count;
            foreach (var image in CompressImageCollection)
            {
                CompressImage = image;
                Optimize = new Optimize(CompressImage);

                FunctionResult = Optimize.VgyUpload();
                if (FunctionResult == FunctionResult.Done)
                {
                    RaisePropertyChanged("CompressImage");
                    CurrentProgress += progressChunk * 50 / 100;

                    FunctionResult = Optimize.ResmushOptimize();
                    if (FunctionResult == FunctionResult.Done)
                    {
                        CurrentProgress += progressChunk * 25 / 100;

                        FunctionResult = Optimize.ResmushDownload();
                        if (FunctionResult == FunctionResult.Done)
                        {
                            RaisePropertyChanged("CompressImage");
                            CurrentProgress += progressChunk * 25 / 100;

                            //VgyDelete(); //Not implemented yet

                            Thread.Sleep(500); //Wait for user to observe the changes
                        }
                    }
                }
                if (FunctionResult != FunctionResult.Done)
                {
                    MessageBox.Show(FunctionResult.ToDisplay(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (FunctionResult == FunctionResult.Done)
            {
                CurrentProgress = 100;
                MessageBox.Show("Everything has been optimized.", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CurrentProgress = 0;
            }
        }
    }
}
