using System.ComponentModel.DataAnnotations;

namespace ImageOptimizer.Model
{
    public enum FunctionResult : byte
    {
        [Display(Name = "An Error Occurred.")]
        Error = 0,

        [Display(Name = "It's All Successfully Done.")]
        Done = 1,

        [Display(Name = "Upload Failed. There Maybe Internet Connection Problems.")]
        UploadFailed = 2,

        [Display(Name = "Download Failed. There Maybe Internet Connection or File Access Problems.")]
        DownloadFailed = 4,

        [Display(Name = "File Access Denied.")]
        FileAccessDenied = 8,
    }
}
