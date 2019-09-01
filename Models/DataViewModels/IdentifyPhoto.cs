using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace plantCamera.Models.DataViewModels
{
    [Table("IdentifyPhoto")]
    public class IdentifyPhoto
    {
        [Key]
        public int IdentifyPhotoId { get; set; }

        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "上传图片")]
        public string IdentifyPhotoLink { get; set; }

        [Display(Name = "识别结果")]
        public string IdentifyName { get; set; }

        [Display(Name = "识别时间")]
        public string IdentifyTime { get; set; }

        [Display(Name = "识别率")]
        public string IdentifyRant { get; set; }

        [Display(Name = "认同")]
        public bool sure { get; set; }
    }

}