using System.ComponentModel.DataAnnotations;

namespace ImageOptimizer.Model
{
    public enum FileSize : byte
    {
        [Display(Name = "B")]
        Byte = 0,

        [Display(Name = "KB")]
        KiloByte = 1,

        [Display(Name = "MB")]
        MegaByte = 2,

        [Display(Name = "GB")]
        GigaByte = 2
    }
}
