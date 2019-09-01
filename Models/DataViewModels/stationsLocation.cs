using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using plantCamera.Models.DataViewModels;

namespace plantCamera.Models.DataViewModels
{
    [Table("stationsLocation")]
    public class stationsLocation
    {
        [Key]
        public string 站点 { get; set; }


        [Display(Name = "省份")]
        public string 省份 { get; set; }

        [Display(Name = "坐标经度")]
        public float 坐标经度 { get; set; }

        [Display(Name = "坐标纬度")]
        public float 坐标纬度 { get; set; }    

    }

}