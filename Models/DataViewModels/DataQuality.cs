using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using plantCamera.Models.DataViewModels;

namespace plantCamera.Models.DataViewModels
{
    [Table("DataQuality")]
    public class DataQuality
    {
        [Key]
        public int DataId { get; set; }

        [Display(Name = "植物名")]
        public string plantName { get; set; }

        [Display(Name = "站点数")]
        public int pointCount { get; set; }

        [Display(Name = "经度差")]
        public float lonDiff { get; set; }

        [Display(Name = "纬度差")]
        public float latDiff { get; set; }

        [Display(Name = "数据量")]
        public int dataCount { get; set; }

        [Display(Name = "最远年份")]
        public string originalYear { get; set; }

        [Display(Name = "最近年份")]
        public string lastYear { get; set; }
    }

}