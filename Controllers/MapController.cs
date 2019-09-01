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
    
    public class MapController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ApplicationDbContext _context;

        public MapController(IHttpContextAccessor httpContextAccessor,ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        
        public IActionResult windowsPop(){   
            return View();
        }

        //
        // GET: /Map/MyPhotoMap
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MyPhotoMap()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["UserName"] = username;

            var Photolist = await GetPhotosAsync(userid);

            List<markerResult> markers = new List<markerResult>();
            foreach(var item in Photolist)
            {
                markerResult marker = new markerResult();
                
                decimal[] lnglat = new decimal[2];
                lnglat[0] = item.PhotoLongitude;
                lnglat[1] = item.PhotoLatitude;
                marker.lnglat = lnglat;
                marker.image = item.PhotoLink;
                marker.plant =item.PhotoPlant;
                marker.PhotoPhenology =item.PhotoPhenology;
                marker.PhotoTakeTime =item.PhotoTakeTime;
                marker.Temperature1 =item.Temperature;
                marker.Weather1 =item.Weather;
                markers.Add(marker);
            }
            ViewBag.mapMakers = Newtonsoft.Json.JsonConvert.SerializeObject(markers);
            return View();
        }
        
        //
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PlantSearch()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            var Photolist = await GetAllPhotosAsync();

            List<markerResult> markers = new List<markerResult>();
            foreach(var item in Photolist)
            {
                markerResult marker = new markerResult();
                
                decimal[] lnglat = new decimal[2];
                lnglat[0] = item.PhotoLongitude;
                lnglat[1] = item.PhotoLatitude;
                marker.lnglat = lnglat;
                marker.image = item.PhotoLink;
                marker.plant =item.PhotoPlant;
                marker.PhotoPhenology =item.PhotoPhenology;
                marker.PhotoTakeTime =item.PhotoTakeTime;
                marker.Temperature1 =item.Temperature;
                marker.Weather1 =item.Weather;
                marker.PhotoLink=item.PhotoLink;
                markers.Add(marker);
            }
            ViewBag.mapAllMakers = Newtonsoft.Json.JsonConvert.SerializeObject(markers);

            var Stationslist = await GetStationLocation("日本樱花");
            ViewBag.stationsLocation = Newtonsoft.Json.JsonConvert.SerializeObject(Stationslist);
            return View();
        }

         // POST: /Map/PlantSearch
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlantSearch(string plant)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            var Photolist = await GetPhotoPlantAsync(plant);

            List<markerResult> markers = new List<markerResult>();
            foreach(var item in Photolist)
            {
                markerResult marker = new markerResult();
                
                decimal[] lnglat = new decimal[2];
                lnglat[0] = item.PhotoLongitude;
                lnglat[1] = item.PhotoLatitude;
                marker.lnglat = lnglat;
                marker.image = item.PhotoLink;
                marker.plant =item.PhotoPlant;
                marker.PhotoPhenology =item.PhotoPhenology;
                marker.PhotoTakeTime =item.PhotoTakeTime;
                marker.Temperature1 =item.Temperature;
                marker.Weather1 =item.Weather;
                marker.PhotoLink=item.PhotoLink;
                markers.Add(marker);
            }
            ViewBag.mapAllMakers = Newtonsoft.Json.JsonConvert.SerializeObject(markers);
            
            return View();
        } 

        //
        // GET: /Map/PlantMap
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PlantMap()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var userid = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["UserName"] = username;

            var Photolist = await GetAllPhotosAsync();

            List<markerResult> markers = new List<markerResult>();
            foreach(var item in Photolist)
            {
                markerResult marker = new markerResult();
                
                decimal[] lnglat = new decimal[2];
                lnglat[0] = item.PhotoLongitude;
                lnglat[1] = item.PhotoLatitude;
                marker.lnglat = lnglat;
                marker.image = item.PhotoLink;
                marker.plant =item.PhotoPlant;
                marker.PhotoPhenology =item.PhotoPhenology;
                marker.PhotoTakeTime =item.PhotoTakeTime;
                marker.Temperature1 =item.Temperature;
                marker.Weather1 =item.Weather;
                markers.Add(marker);
            }
            ViewBag.mapMakers = Newtonsoft.Json.JsonConvert.SerializeObject(markers);
            return View();
        }

        private Task<List<PhotoInformation>> GetPhotosAsync(string id)//用户图片清单
        {
            List<PhotoInformation> photosList = new List<PhotoInformation>();
            photosList = _context.PhotoInformation.Where(u => u.Id == id && u.DeleteFlag == false).OrderBy(u => u.PhotoInformationId).ToList();
            return Task.FromResult(photosList);//用来创建一个带返回值的、已完成的Task
        }
         private Task<List<PhotoInformation>> GetAllPhotosAsync()//所有图片的清单
        {
            List<PhotoInformation> photosList = new List<PhotoInformation>();
            photosList = _context.PhotoInformation.Where(u => u.DeleteFlag == false).OrderBy(u => u.PhotoInformationId).ToList();
            return Task.FromResult(photosList);//用来创建一个带返回值的、已完成的Task
        }

        private Task<List<stationsLocation>> GetStationLocation(string species)
        {
            List<allpoint> pointlist = new List<allpoint>();
            pointlist = _context.allpoint.Where(u => u.植物名 == species).ToList();
            IEnumerable<IGrouping<string, allpoint>> grouplist = pointlist.GroupBy(u => u.站点);
            List<stationsLocation> stationsList = _context.stationsLocation.ToList();
            List<stationsLocation> pointsList = new List<stationsLocation>();
            foreach(IGrouping<string, allpoint> item in grouplist)
            {
                List<allpoint> point = item.ToList();
                var pointname = point[0].站点;
                var pointlocation = stationsList.Where(u => u.站点 == pointname).FirstOrDefault();
                if(pointlocation != null){
                    pointsList.Add(pointlocation);
                }
            }
            return Task.FromResult(pointsList);//用来创建一个带返回值的、已完成的Task
        }

        private Task<List<allpoint>> GetPlantsAsync()//获取所有城市植物物种
        { 
            
            List<allpoint> plantsList = new List<allpoint>();
            List<allpoint> onlyOnePlant = new List<allpoint>();
            //plantsList = _context.allpoint.Where(u => u.植物名  == "侧柏").ToList();//-------------------tolist是个啥--------咋写一个复杂的sql------------数据库迁移是啥--------------vscode环境部署
            //plantsList = _context.allpoint.Select(*from allpoint a).Where not exists (Select * from allpoint b Where b.植物名 = a.植物名 and b.id < a.id).ToList();
            
            plantsList = _context.allpoint.Where(a => !_context.allpoint.Any(b => (b.植物名==a.植物名 && b.id<a.id))).OrderBy(b => b.植物名).ToList();
            return Task.FromResult(plantsList);//用来创建一个带返回值的、已完成的Task
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<List<PhotoInformation>> GetPhotoPlantAsync(string plant){
             List<PhotoInformation>photoplantList = new List<PhotoInformation>();
             photoplantList = _context.PhotoInformation.Where(u => u.PhotoPlant == plant && u.DeleteFlag == false).OrderBy(u => u.PhotoTakeTime).ToList();
            return Task.FromResult(photoplantList);//用来创建一个带返回值的、已完成的Task
        }

        public class markerResult
        {
            public Array lnglat { get; set; }
            public string image { get; set; }
            public string plant { get; set; }
            public string  PhotoPhenology { get; set; }
            public string  PhotoTakeTime { get; set; }
            public float  Temperature1 { get; set; }
            public string  Weather1 { get; set; }
            public string  PhotoLink { get; set; }
            

        }

    }
}


        

   