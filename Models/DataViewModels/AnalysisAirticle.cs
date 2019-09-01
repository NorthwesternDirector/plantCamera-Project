using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace plantCamera.Models.DataViewModels
{
    [Table("AnalysisAirticle")]
    public class AnalysisAirticle
    {
        [Key]
        public int aid { get; set; }

        [Display(Name = "作者")]
        public string Author { get; set; }

        [Display(Name = "上传者")]
        public string AirticleUser { get; set; }

        [Display(Name = "文章标题")]
        public string AirticleTitle { get; set; }

        [Display(Name = "发布时间")]
        public string AirticleTime { get; set; }

        [Display(Name = "内容")]
        public string Airticle { get; set; }

        [Display(Name = "附图名")]
        public string AirticlePhoto { get; set; }

        [Display(Name = "附图地址")]
        public string AirticlePhotoLink { get; set; }

        [Display(Name = "附文件名")]
        public string AirticleDoc { get; set; }

        [Display(Name = "附文件地址")]
        public string AirticleDocLink { get; set; }
    }

}