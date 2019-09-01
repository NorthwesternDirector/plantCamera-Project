using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using plantCamera.Models.DataViewModels;

namespace plantCamera.Models.DataViewModels
{
    [Table("AlbumB")]
    public class AlbumB
    {
        [Key]
        public int AlbumBId { get; set; }
        
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "相册名称")]
        public string AlbumsName { get; set; }

        [Display(Name = "封面地址")]
        public string CoverLink { get; set; }

        [Display(Name = "相册简介")]
        public string AlbumText { get; set; }

        [Display(Name = "创建时间")]
        public string AlbumTime { get; set; }
    }

}