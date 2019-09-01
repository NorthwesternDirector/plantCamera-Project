using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace plantCamera.Models.DataViewModels
{
    [Table("PhotoInformation")]
    public class PhotoInformation
    {
        [Key]
        public int PhotoInformationId { get; set; }

        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "相册名")]
        public string AlbumName { get; set; }

        [Display(Name = "图像地址")]
        public string PhotoLink { get; set; }

        [Display(Name = "图像上传时间")]
        public string PhotoUploadTime { get; set; }

        [Display(Name = "删除")]
        public bool DeleteFlag { get; set; }

        [Display(Name = "封面")]
        public bool Cover { get; set; }

        [Display(Name = "图片质量分")]
        public int PhotoGrade { get; set; }

        [Display(Name = "图片来源")]
        public string PhotoSource { get; set; }

        //物候信息
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "图像拍摄时间")]
        public string PhotoTakeTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "图像经度")]
        public decimal PhotoLongitude { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "图像纬度")]
        public decimal PhotoLatitude { get; set; }

        [Display(Name = "图像海拔")]
        public float PhotoHeight { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "植物品种")]
        public string PhotoPlant { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "候应现象")]
        public string PhotoPhenology { get; set; }

        [Display(Name = "农历时间")]
        public string LunarTime { get; set; }

        [Display(Name = "季节")]
        public string Season { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "温度")]
        public float Temperature { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "天气现象")]
        public string Weather { get; set; }

        [Display(Name = "植物生长简况")]
        public string PlantText { get; set; }
    }

}