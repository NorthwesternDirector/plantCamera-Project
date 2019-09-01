using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using plantCamera.Models.DataViewModels;

namespace plantCamera.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CreateTime { get; set; }
        public string CreateUserIP { get; set; }
        public int LoginTimes { get; set; }
        public string LastLoginTime { get; set; }
        public string InviteCode { get; set; }

        public virtual AlbumA AlbumA { get; set; }
    }
}
