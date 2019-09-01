using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace plantCamera.Models.DataViewModels
{
    [Table("AlbumA")]
    public class AlbumA
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "储存地址")]
        public string AlbumLink { get; set; }

        [Display(Name = "相册总数")]
        public int AlbumNumber { get; set; }

        [Display(Name = "图片总数")]
        public int PhotoNumber { get; set; }

        [Display(Name = "收藏文章数")]
        public int StarNumber { get; set; }

        [Display(Name = "头像尺寸1")]
        public string ProfileLink1 { get; set; }

        [Display(Name = "头像尺寸2")]
        public string ProfileLink2 { get; set; }

        [Display(Name = "头像尺寸3")]
        public string ProfileLink3 { get; set; }
    }

}