using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using plantCamera.Models.DataViewModels;

namespace plantCamera.Models.DataViewModels
{
    [Table("allpoint")]
    public class allpoint
    {
        [Key]
        public int id { get; set; }

        public string 植物名 { get; set; }

        public string 年份 { get; set; }
        
        public string 站点 { get; set; }

        public string 注记 { get; set; }

        [Display(Name = "叶芽开始膨大期")]
        public string 叶芽开始膨大期 { get; set; }

        [Display(Name = "叶芽开放期")]
        public string 叶芽开放期 { get; set; }

        [Display(Name = "花芽开始膨大期")]
        public string 花芽开始膨大期 { get; set; }

        [Display(Name = "花芽开放期")]
        public string 花芽开放期 { get; set; }
        
        [Display(Name = "开始展叶期")]
        public string 开始展叶期 { get; set; }
         
        [Display(Name = "展叶盛期")]
        public string 展叶盛期 { get; set; }
        
        [Display(Name = "花序或花蕾出现期")]
        public string 花序或花蕾出现期 { get; set; }
        
        [Display(Name = "开花始期")]
        public string 开花始期 { get; set; }
         
        [Display(Name = "开花盛期")]
        public string 开花盛期 { get; set; }

        [Display(Name = "开花末期")]
        public string 开花末期 { get; set; }

        [Display(Name = "第二次开花期")]
        public string 第二次开花期 { get; set; }

        [Display(Name = "果实成熟期")]
        public string 果实成熟期 { get; set; }

        [Display(Name = "果实脱落开始期")]
        public string 果实脱落开始期 { get; set; }

        [Display(Name = "果实脱落末期")]
        public string 果实脱落末期 { get; set; }

        [Display(Name = "叶开始变色期")]
        public string 叶开始变色期 { get; set; }

        [Display(Name = "叶全部变色期")]
        public string 叶全部变色期 { get; set; }

        [Display(Name = "开始落叶期")]
        public string 开始落叶期 { get; set; }

        [Display(Name = "落叶末期")]
        public string 落叶末期 { get; set; }      

    }

}