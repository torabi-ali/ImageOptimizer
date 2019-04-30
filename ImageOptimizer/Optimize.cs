using ImageOptimizer.Helpers;
using ImageOptimizer.Model;
using ImageOptimizer.Utility;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ImageOptimizer
{
    public class Optimize
    {
        private static readonly HttpClient vgyClient = new HttpClient();
        private readonly CompressImage compressImage;

        public Optimize(CompressImage CompressImage)
        {
            compressImage = CompressImage;

            if (compressImage.DestinationFileFullPath != null)
            {
                File.Delete(compressImage.DestinationFileFullPath);
            }

            if (vgyClient.BaseAddress == null)
            {
                vgyClient.BaseAddress = new Uri("https://vgy.me/");
                vgyClient.DefaultRequestHeaders.Accept.Clear();
                vgyClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        #region Vgy
        public FunctionResult VgyUpload()
        {
            HttpContent content = new StringContent("image");
            var form = new MultipartFormDataContent();

            FileStream stream;
            try
            {
                stream = new FileStream(compressImage.SourceFileFullPath, FileMode.Open);
            }
            catch (Exception ex)
            {
                ex.Log();
                return FunctionResult.FileAccessDenied;
            }

            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = compressImage.SourceFileName
            };
            form.Add(content);

            try
            {
                var response = vgyClient.PostAsync("upload/", form).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var vgyResult = result.DeserializeJson<VgyResult>();

                if (vgyResult.error)
                {
                    throw new Exception(result);
                }
                else
                {
                    compressImage.SourceUrl = vgyResult.image;
                    compressImage.DeleteUrl = vgyResult.delete;
                    compressImage.SourceSize = vgyResult.size;
                    return FunctionResult.Done;
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return FunctionResult.UploadFailed;
            }
        }

        public FunctionResult VgyDelete()
        {
            var data = compressImage.DeleteUrl;
            var url = $"https://vgy.me/delete?{data}";
            var request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return FunctionResult.Done;
            }
            catch (Exception ex)
            {
                ex.Log();
                return FunctionResult.Error;
            }
        }
        #endregion

        #region Resmush
        public FunctionResult ResmushOptimize()
        {
            var data = $"img={compressImage.SourceUrl}&qlty={Properties.Settings.Default.Quality}";
            var url = $"http://api.resmush.it/ws.php?{data}";
            var request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                var result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var resmushResult = result.DeserializeJson<ResmushResult>();

                if (resmushResult == null)
                {
                    throw new Exception(result);
                }
                else
                {
                    compressImage.DestinationUrl = resmushResult.dest;
                    compressImage.DestinationSize = resmushResult.dest_size;
                    compressImage.CalculatePercentageSaved();
                    compressImage.CalculateViewSize();
                    return FunctionResult.Done;
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return FunctionResult.Error;
            }
        }

        public FunctionResult ResmushDownload()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(compressImage.DestinationUrl, compressImage.DestinationFileFullPath);
                }
                return FunctionResult.Done;
            }
            catch (Exception ex)
            {
                ex.Log();
                return FunctionResult.DownloadFailed;
            }
        }
        #endregion
    }
}
