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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using plantCamera.Models;
using plantCamera.Models.DataViewModels;
using plantCamera.Data;
using Newtonsoft.Json;
using plantCamera.Services;

namespace plantCamera.Controllers
{
    public class CameraController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _environment;
        private ApplicationDbContext _context;

        public CameraController(
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            IHostingEnvironment environment)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _environment = environment;
        }

        //
        // GET: /Camera/Album
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Album()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["UserName"] = username;

            var Albumlist = await GetAlbumAsync(userid);
            ViewBag.AlbumList = Albumlist;

            var Photolist = await GetPhotosAsync(userid);
            
            return View(Photolist);
        }

        //
        // POST: /Camera/Album
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Album(string name,string text)
        {
            AlbumB model = new AlbumB();
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["UserName"] = username;
            var createTime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
            Random ran = new Random();
            int n = ran.Next(1,7);
            string link = "../images/album-cover/album-" + n + ".jpg";

            model.Id = userId;
            model.AlbumsName = name;
            model.AlbumText = text;
            model.CoverLink = link;
            model.AlbumTime = createTime;
            
            var nameresult = _context.Set<AlbumB>().Where(u => u.Id == userId).FirstOrDefault(u => u.AlbumsName == model.AlbumsName);
            if(nameresult == null){
                try{
                _context.AlbumB.Add(model);
                await _context.SaveChangesAsync();
                ViewBag.addResult = "添加成功";
                }catch{
                    ViewBag.addResult = "添加失败，数据库连接错误";
                }
            }else{
                ViewBag.addResult = "添加失败，相册名重复";
            } 

            var Albumlist = await GetAlbumAsync(userId);
            ViewBag.AlbumList = Albumlist;

            var Photolist = await GetPhotosAsync(userId);
            return View(Photolist);
        }

        //
        // POST: /Camera/DeleteAlbum
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAlbum([FromBody]int? id)
        {
            if(id != null){
                var alb = _context.AlbumB.Where(u => u.AlbumBId == id).FirstOrDefault();
                if(alb != null){
                    var albname = alb.AlbumsName;
                    var albid = alb.Id;
                    _context.AlbumB.Remove(alb);
                    var pho = _context.PhotoInformation.Where(u => u.AlbumName == albname && u.Id == albid).ToList();
                    if(pho.Count != 0){
                        pho.ForEach(c => c.AlbumName = "默认相册");
                    }
                    await _context.SaveChangesAsync();
                    return Json(Url.Action("Album","Camera"));
                }else{
                    return Json("出错了");
                }
            }else{
                return Json("出错了");
            }
        }

        //
        // POST: /Camera/ChangeAlbum
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeAlbum([FromBody]AlbumResult res)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try{
                var alb = _context.AlbumB.Where(u => u.AlbumBId == res.aid && u.AlbumsName == res.preName).ToList();
                if(alb != null){
                    var nameresult = _context.AlbumB.Where(u => u.Id == userId).FirstOrDefault(u => u.AlbumsName == res.postName);
                    if(nameresult == null){
                        alb.ForEach(c => c.AlbumsName = res.postName);
                        var pho = _context.PhotoInformation.Where(u => u.AlbumName == res.preName).ToList();
                        if(pho.Count != 0){
                            pho.ForEach(c => c.AlbumName = res.postName);
                        }
                        await _context.SaveChangesAsync();
                        return Json(Url.Action("Album","Camera"));

                    }else{
                        return Json("相册名重复");
                    }
                }else{
                    return Json("出错了");
                }
            }catch{
                return Json("出错了");
            }  
        }

        public class AlbumResult  
        {
            public string preName { get; set; }
            public string postName { get; set; }
            public int aid { get; set; }
        }

        //
        // POST: /Camera/DeletePhoto
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeletePhoto([FromBody]int? id)
        {
            if(id != null){
                var pho = _context.PhotoInformation.Where(u => u.PhotoInformationId == id).FirstOrDefault();
                if(pho != null){
                    pho.DeleteFlag = true;
                    await _context.SaveChangesAsync();
                    return Json(Url.Action("Album","Camera"));
                }else{
                    return Json("出错了");
                }
            }else{
                return Json("出错了");
            }
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
            photosList = _context.PhotoInformation.Where(u => u.Id == id && u.DeleteFlag == false).OrderBy(u => u.PhotoInformationId).ToList();
            foreach(var item in photosList)
            {
                if(item.AlbumName == "" || item.AlbumName == null){
                    item.AlbumName = "默认相册";
                }
            }
            return Task.FromResult(photosList);
        }

        //
        // GET: /Camera/Upload
        public IActionResult Upload()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;
            return View();
        }

        //
        // POST: /Camera/Upload
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(PhotoInformation model)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            var uploadTime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
            var season = Determineseason(model.PhotoTakeTime);
            if (ModelState.IsValid){
                model.Id = userId;
                model.PhotoUploadTime = uploadTime;
                model.DeleteFlag = false;
                model.PhotoGrade = 100;
                model.PhotoSource = "网页端";
                model.Season = season;

                try{
                    _context.PhotoInformation.Add(model);
                    _context.SaveChanges();
                    ModelState.AddModelError("uploadResult", "上传成功");
                }catch{
                    ModelState.AddModelError("uploadResult", "上传失败，数据库连接错误");
                }
                
            }else{
                ModelState.AddModelError("uploadResult", "上传失败，图片信息格式有误");
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult UpLoadfile()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            uploadResult result = new uploadResult();
            try{
                IFormFile oFile = Request.Form.Files["uploadfiles"];
                Stream sm=oFile.OpenReadStream();
                result.fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + oFile.FileName;
                result.filePath = "../../user/" + userId + "/" + result.fileName;

                var filepath = _environment.ContentRootPath + "/wwwroot/user/" + userId;
                if(!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                string filename = filepath + "/" + result.fileName;
                FileStream fs = new FileStream(filename , FileMode.Create);
                byte[] buffer = new byte[sm.Length];
                sm.Read(buffer , 0 , buffer.Length);
                fs.Write(buffer , 0 , buffer.Length);
                fs.Dispose();

                var imgpath = _environment.ContentRootPath + "/wwwroot/user/" + userId + "/" + result.fileName;
                using (Image<Rgba32> image = Image.Load(imgpath))
                {
                    var exif = image.MetaData.ExifProfile;
                    if(exif != null){
                        if(exif.GetValue(ExifTag.DateTime) != null && exif.GetValue(ExifTag.GPSLongitude) != null){
                            result.takeTime = exif.GetValue(ExifTag.DateTime).ToString();
                            var Lng = exif.GetValue(ExifTag.GPSLongitude).ToString();
                            var Lat = exif.GetValue(ExifTag.GPSLatitude).ToString();
                            result.takeLng = ChangeCoordinate(exif,Lng);
                            result.takeLat = ChangeCoordinate(exif,Lat);
                        }else{
                            result.error = "图片EXIF信息不全";
                            // result.takeTime = "2018:03:04 22:28:56";
                            // Random r = new Random();
                            // int rn = r.Next(80,100);
                            // int rg = r.Next(0,35);
                            // decimal lng = Convert.ToDecimal(120.49) + Convert.ToDecimal(rn)/10000;
                            // decimal lat = Convert.ToDecimal(36.16) + Convert.ToDecimal(rg)/10000;
                            // result.takeLng = lng;
                            // result.takeLat = lat;
                        }
                    }else{
                        result.error = "该图片非手机或照相机拍摄的原图";
                        // result.takeTime = "2018:03:04 22:28:56";
                        // Random r = new Random();
                        // int rn = r.Next(80,100);
                        // int rg = r.Next(0,35);
                        // decimal lng = Convert.ToDecimal(120.49) + Convert.ToDecimal(rn)/10000;
                        // decimal lat = Convert.ToDecimal(36.16) + Convert.ToDecimal(rg)/10000;
                        // result.takeLng = lng;
                        // result.takeLat = lat;
                    }
                }
            }
            catch(Exception ex) {
                result.error = ex.Message;
            }

            return Json(result);
        }

        public class uploadResult
        {
            public string fileName { get; set; }
            public string filePath { get; set; }
            public string takeTime { get; set; }
            public decimal takeLng { get; set; }
            public decimal takeLat { get; set; }
            public string error { get; set; }
        }

        public decimal ChangeCoordinate(ExifProfile exif, string coordinate){
            if(coordinate != null){
                string[] value = coordinate.Split(new Char[]{' '});
                var du = value.GetValue(0).ToString();
                var fen = value.GetValue(1).ToString();
                var miao = value.GetValue(2).ToString();
                string[] miaoarr = miao.Split(new Char[]{'/'});
                decimal degree = Convert.ToDecimal(du);
                decimal minute = Convert.ToDecimal(fen);
                decimal second = Convert.ToDecimal(miaoarr.GetValue(0))/Convert.ToDecimal(miaoarr.GetValue(1));
                decimal fencoord = minute + second/60;
                decimal coord = degree + fencoord/60;
                coord = Math.Round(coord,9);
                return coord;
            }else{
                decimal error = 0;
                return error;
            }
        }

        public string Determineseason(string datetime){
            string[] value = datetime.Split(new Char[]{' '});
            var datestring = value.GetValue(0).ToString();
            string[] datearray = datestring.Split(new Char[]{':'});
            var year = datearray.GetValue(0).ToString();
            var month = datearray.GetValue(1).ToString();
            var day = datearray.GetValue(2).ToString();
            DateTime date = new DateTime(Convert.ToInt32(year),Convert.ToInt32(month),Convert.ToInt32(day));
            string season = "";

            DateTime springStart = new DateTime(Convert.ToInt32(year),3,21);
            DateTime summerStart = new DateTime(Convert.ToInt32(year),6,21);
            DateTime autumnStart = new DateTime(Convert.ToInt32(year),9,21);
            DateTime winterStart = new DateTime(Convert.ToInt32(year),12,21);

            if(date >= springStart && date < summerStart){
                season = "春季";
            }else if(date >= summerStart && date < autumnStart){
                season = "夏季";
            }else if(date >= autumnStart && date < winterStart){
                season = "秋季";
            }else if(date >= winterStart || date < springStart){
                season = "冬季";
            }else{
                season = "error";
            }
            
            return season;
        }

        //
        // GET: /Camera/ReadAlbum
        [HttpGet]
        [AllowAnonymous]
        public string ReadAlbum()
        {
            List<AlbumB> list = new List<AlbumB>();
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            list = _context.Set<AlbumB>().Where(u => u.Id == userId).OrderBy(u => u.AlbumBId).ToList();
            string liststr = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return liststr;
        }
    }
}