using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Cryptography;
using Newtonsoft.Json;
using plantCamera.Models;
using plantCamera.Models.DataViewModels;
using plantCamera.Data;

namespace plantCamera.Components
{
    [ViewComponent(Name = "Partial")]

    public class PartialViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext  _context;
        private IHttpContextAccessor _httpContextAccessor;
        public PartialViewComponent(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        } 
        public async Task<IViewComponentResult> InvokeAsync(string albumname)
        {
            var photoslist = await GetPhotosAsync(albumname);
            ViewBag.phostr = Newtonsoft.Json.JsonConvert.SerializeObject(photoslist);
            return View();
        }

        private Task<List<PhotoInformation>> GetPhotosAsync(string name)
        {
            List<PhotoInformation> photosList = new List<PhotoInformation>();
            var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            photosList = _context.PhotoInformation.Where(u => u.Id == userid && u.AlbumName == name).OrderBy(u => u.PhotoInformationId).ToList();
            return Task.FromResult(photosList);
        }
    }
}