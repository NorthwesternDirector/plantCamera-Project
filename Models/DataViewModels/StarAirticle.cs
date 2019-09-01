using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace plantCamera.Models.DataViewModels
{
    [Table("StarAirticle")]
    public class StarAirticle
    {
        [Key]
        public int StarAirticleId { get; set; }

        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "收藏文章ID")]
        public int aid { get; set; }

        [Display(Name = "收藏时间")]
        public string StarTime { get; set; }
    }

}