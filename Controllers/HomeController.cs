using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using plantCamera.Models;
using plantCamera.Models.AccountViewModels;
using plantCamera.Models.DataViewModels;
using plantCamera.Data;
using Newtonsoft.Json;
using plantCamera.Services;

namespace plantCamera.Controllers
{
    public class HomeController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _environment;
        private ApplicationDbContext _context;

        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            IHostingEnvironment environment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["UserName"] = username;
            
            var Albumlist = await GetAlbumAsync(userid);
            ViewBag.AlbumList = Albumlist;

            var Photolist = await GetPhotosAsync(userid);
            ViewBag.PhotoNumber = Photolist.Count;

            var Datelist = await GetTimeAsync(userid);
            foreach(var u in Datelist)
            {
                Console.WriteLine(u.PhotoTakeTime);
            }
            ViewBag.DateNumber = Datelist.Count;
            return View(Photolist);
        }

        private Task<List<AlbumB>> GetAlbumAsync(string id)
        {
            List<AlbumB> albumList = new List<AlbumB>();
            albumList = _context.AlbumB.Where(u => u.Id == id).OrderBy(u => u.AlbumBId).ToList();

            return Task.FromResult(albumList);
        }

        private Task<List<PhotoInformation>> GetPhotosAsync(string id)
        {
            List<PhotoInformation> photosList = new List<PhotoInformation>();
            photosList = _context.PhotoInformation.Where(u => u.Id == id).OrderByDescending(u => u.PhotoUploadTime).ToList();
            return Task.FromResult(photosList);
        }

        private Task<List<PhotoInformation>> GetTimeAsync(string id)
        {
            List<PhotoInformation> timeList = new List<PhotoInformation>();
            timeList = _context.PhotoInformation.Distinct(u => u.PhotoTakeTime.Substring(0,10)).ToList();
            return Task.FromResult(timeList);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult DownloadAPP()
        {
            var filepath = _environment.ContentRootPath + "/wwwroot/app/PlantCamera.apk"; 
            var stream = System.IO.File.OpenRead(filepath);  
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(filepath));
        }

    }
}