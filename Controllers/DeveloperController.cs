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
    public class DeveloperController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ApplicationDbContext _context;

        public DeveloperController(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public IActionResult Contact()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            //getPointData();
            return View();
        }

        public async Task<IActionResult> DataReview()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            var Photolist = await GetPhotosAsync();
            
            return View(Photolist);
        }

        private Task<List<PhotoInformation>> GetPhotosAsync()
        {
            List<PhotoInformation> photosList = new List<PhotoInformation>();
            photosList = _context.PhotoInformation.Where(u => u.DeleteFlag == false).OrderBy(u => u.PhotoInformationId).Take(7).ToList();
            foreach(var item in photosList)
            {
                if(item.AlbumName == "" || item.AlbumName == null){
                    item.AlbumName = "默认相册";
                }
            }
            return Task.FromResult(photosList);
        }

        public IActionResult UsersManage()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;
            
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DataOutputh()
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

            var Stationslist = await GetStationLocation("垂柳");
            ViewBag.stationsLocation = Newtonsoft.Json.JsonConvert.SerializeObject(Stationslist);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DataOutput(string plant)
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

        [HttpPost]
        [AllowAnonymous]
        public Task<List<PhotoInformation>> GetPhotoPlantAsync(string plant){
             List<PhotoInformation>photoplantList = new List<PhotoInformation>();
             photoplantList = _context.PhotoInformation.Where(u => u.PhotoPlant == plant && u.DeleteFlag == false).OrderBy(u => u.PhotoTakeTime).ToList();
            return Task.FromResult(photoplantList);//用来创建一个带返回值的、已完成的Task
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

        // public async void getPointData()
        // {
        //     List<stationsLocation> locationList = new List<stationsLocation>();
        //     locationList = _context.stationsLocation.ToList();
        //     IEnumerable<IGrouping<string, allpoint>> plant = _context.allpoint.GroupBy(u => u.植物名);
        //     foreach(IGrouping<string, allpoint> item in plant){
        //         List<allpoint> plantlist = item.ToList();
        //         DataQuality plantquality = new DataQuality();
        //         plantquality.plantName = plantlist[0].植物名;
        //         plantquality.dataCount = plantlist.Count();

        //         IEnumerable<IGrouping<string, allpoint>> plantgroup = plantlist.GroupBy(u => u.站点);
        //         plantquality.pointCount = plantgroup.Count();
        //         if(plantgroup.Count() > 1){
        //             List<lonlat> lonlatList = new List<lonlat>();
        //             foreach(IGrouping<string, allpoint> u in plantgroup){
        //                 List<allpoint> ulist = u.ToList();
        //                 var pointname = ulist[0].站点;
        //                 var pointvalue = locationList.FirstOrDefault(m => m.站点 == pointname);
        //                 if(pointvalue != null){
        //                     lonlat lonlat = new lonlat();
        //                     var lon = pointvalue.坐标经度 - (float)101.57;
        //                     lonlat.lon = lon;
        //                     var lat = pointvalue.坐标纬度 - (float)21.48;
        //                     lonlat.lat = lat;
        //                     lonlatList.Add(lonlat);
        //                 }else{
        //                     Console.WriteLine(pointname);
        //                 }
        //             }
        //             List<lonlat> lono = lonlatList.OrderBy(m => m.lon).ToList();
        //             List<lonlat> lond = lonlatList.OrderByDescending(m => m.lon).ToList();
        //             var lonc = lond[0].lon - lono[0].lon;
        //             plantquality.lonDiff = lonc;
        //             List<lonlat> lato = lonlatList.OrderBy(m => m.lat).ToList();
        //             List<lonlat> latd = lonlatList.OrderByDescending(m => m.lat).ToList();
        //             var latc = latd[0].lat - lato[0].lat;
        //             plantquality.latDiff = latc;
        //         }else{
        //             plantquality.latDiff = 0;
        //             plantquality.lonDiff = 0;
        //         }

        //         List<allpoint> planto = plantlist.OrderBy(u => u.年份).ToList();
        //         plantquality.originalYear = planto[0].年份;

        //         List<allpoint> plantl = plantlist.OrderByDescending(u => u.年份).ToList();
        //         plantquality.lastYear = plantl[0].年份;

        //         _context.DataQuality.Add(plantquality);
        //     }
        //     await _context.SaveChangesAsync();
        // }

        // private class lonlat{
        //     public float lon { get; set; }
        //     public float lat { get; set; }
        // }
    }
}
