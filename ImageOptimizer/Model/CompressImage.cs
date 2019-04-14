using System;
using System.Linq;

namespace ImageOptimizer.Model
{
    public class CompressImage
    {
        public long Id { get; set; }
        public string SourceFileName { get; }
        public string SourceFilePath { get; }
        public string SourceFileFullPath { get; set; }
        public string SourceUrl { get; set; }
        public int SourceSize { get; set; }
        public string DestinationFileName { get; }
        public string DestinationFilePath { get; }
        public string DestinationFileFullPath { get; }
        public string DestinationUrl { get; set; }
        public int DestinationSize { get; set; }
        public string DeleteUrl { get; set; }
        public string PercentageSaved { get; set; }

        public CompressImage(string fullPath)
        {
            Id = DateTime.Now.Ticks;

            SourceFileFullPath = fullPath;
            SourceFileName = SourceFileFullPath.Split('\\').Last();
            SourceFilePath = SourceFileFullPath.Remove(SourceFileFullPath.Length - SourceFileName.Length, SourceFileName.Length);

            DestinationFilePath = SourceFilePath;
            DestinationFileName = $"Compressed-{SourceFileName}";
            DestinationFileFullPath = $"{DestinationFilePath}{DestinationFileName}";
        }

        public void CalculatePercentageSaved()
        {
            var result = DestinationSize - SourceSize;
            if (result > 0)
            {
                PercentageSaved = $"+{result / 100}% (Keep the original image.)";
            }
            else
            {
                PercentageSaved = $"{result / 100}%";
            }
        }
    }
}
