using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace plantCamera.Models.AccountViewModels
{
    public class RegisterViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "不少于8个字符", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}不能为空")]
        [Display(Name = "邀请码")]
        public string InviteCode { get; set; }

    }
}
