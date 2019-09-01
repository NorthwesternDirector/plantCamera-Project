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
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace plantCamera.Controllers
{
    public class AnalysisController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        IHostingEnvironment _environment;
        private ApplicationDbContext _context;

        public AnalysisController(IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment,ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
            _context = context;
        }

        public IActionResult Topic()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;
            return View();
        }

        //
        // GET: /Analysis/PointsMap
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PointsMap()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            var city = "北京";
            var specie = "侧柏";

            var citySpecies = await GetSpeciesAsync(city);//传输单一城市的植物清单
            IEnumerable<IGrouping<string, allpoint>> plantl = citySpecies.GroupBy(u => u.植物名);
            List<SpieciesResult> species = new List<SpieciesResult>();
            foreach(IGrouping<string, allpoint> item in plantl)
            {
                List<allpoint> plantlist = item.ToList();
                SpieciesResult speciesname = new SpieciesResult();
                speciesname.species = plantlist[0].植物名;
                species.Add(speciesname);
            }
            ViewBag.citySpecies= Newtonsoft.Json.JsonConvert.SerializeObject(species);

            var onlyOnePlant = await GetDatasAsync(city, specie);//传输“单一城市单一植物”数据
            List<plantResult> plantsData = new List<plantResult>();
            foreach(var item in onlyOnePlant)
            {
                plantResult plant = new plantResult();
                plant.datax1 = convertDate(item.叶芽开始膨大期);
                plant.datax2 = convertDate(item.叶芽开放期);
                plant.datax3 = convertDate(item.花芽开始膨大期);
                plant.datax4 = convertDate(item.花芽开放期);
                plant.datax5 = convertDate(item.开始展叶期);
                plant.datax6 = convertDate(item.展叶盛期);
                plant.datax7 = convertDate(item.花序或花蕾出现期);
                plant.datax8 = convertDate(item.开花始期);
                plant.datax9 = convertDate(item.开花盛期);
                plant.datax10 = convertDate(item.开花末期);
                plant.datax11 = convertDate(item.第二次开花期);
                plant.datax12 = convertDate(item.果实成熟期);
                plant.datax13 = convertDate(item.果实脱落开始期);
                plant.datax14 = convertDate(item.果实脱落末期);
                plant.datax15 = convertDate(item.叶开始变色期);
                plant.datax16 = convertDate(item.叶全部变色期);
                plant.datax17 = convertDate(item.开始落叶期);
                plant.datax18 = convertDate(item.落叶末期);
                plant.datay = item.年份+'年';   
                plant.zhuji=item.注记;         
                plantsData.Add(plant);
            }
            ViewBag.onlyPlant = Newtonsoft.Json.JsonConvert.SerializeObject(plantsData);
            return View();
        }

        // POST: /Analysis/PointsMap
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PointsMap(string city, string species)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;
            
            var citySpecies = await GetSpeciesAsync(city);
            IEnumerable<IGrouping<string, allpoint>> plantl = citySpecies.GroupBy(u => u.植物名);
            List<SpieciesResult> specie = new List<SpieciesResult>();
            foreach(IGrouping<string, allpoint> item in plantl)
            {
                List<allpoint> plantlist = item.ToList();
                SpieciesResult speciesname = new SpieciesResult();
                speciesname.species = plantlist[0].植物名;
                specie.Add(speciesname);
            }
            ViewBag.citySpecies= Newtonsoft.Json.JsonConvert.SerializeObject(specie);

            var onlyOnePlant = await GetDatasAsync(city, species);//传输“单一城市单一植物”数据
            List<plantResult> plantsData = new List<plantResult>();
            foreach(var item in onlyOnePlant)
            {
                plantResult plant = new plantResult();
                plant.datax1 = convertDate(item.叶芽开始膨大期);
                plant.datax2 = convertDate(item.叶芽开放期);
                plant.datax3 = convertDate(item.花芽开始膨大期);
                plant.datax4 = convertDate(item.花芽开放期);
                plant.datax5 = convertDate(item.开始展叶期);
                plant.datax6 = convertDate(item.展叶盛期);
                plant.datax7 = convertDate(item.花序或花蕾出现期);
                plant.datax8 = convertDate(item.开花始期);
                plant.datax9 = convertDate(item.开花盛期);
                plant.datax10 = convertDate(item.开花末期);
                plant.datax11 = convertDate(item.第二次开花期);
                plant.datax12 = convertDate(item.果实成熟期);
                plant.datax13 = convertDate(item.果实脱落开始期);
                plant.datax14 = convertDate(item.果实脱落末期);
                plant.datax15 = convertDate(item.叶开始变色期);
                plant.datax16 = convertDate(item.叶全部变色期);
                plant.datax17 = convertDate(item.开始落叶期);
                plant.datax18 = convertDate(item.落叶末期);
                plant.zhuji=item.注记;
                plant.datay = item.年份+'年';            
                plantsData.Add(plant);
            }
            ViewBag.onlyPlant = Newtonsoft.Json.JsonConvert.SerializeObject(plantsData);
            ViewBag.City = Newtonsoft.Json.JsonConvert.SerializeObject(city);
            ViewBag.Species = Newtonsoft.Json.JsonConvert.SerializeObject(species);
            
            return View();
        }

        // GET: /Analysis/stationsAnalysis
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> stationsAnalysis()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            var Stationslist = await GetStationLocation("垂柳");
            ViewBag.stationsLocation = Newtonsoft.Json.JsonConvert.SerializeObject(Stationslist);

            List<allpoint> citySpecies = new List<allpoint>();          
            citySpecies =_context.allpoint.Where(a => a.植物名 == "垂柳").ToList();
            IEnumerable<IGrouping<string, allpoint>> plantl = citySpecies.GroupBy(u => u.年份);
            List<CityResult> year = new List<CityResult>();
            foreach(IGrouping<string, allpoint> item in plantl)
            {
                List<allpoint> plantlist = item.ToList();
                CityResult yearname = new CityResult();
                yearname.city = plantlist[0].年份;
                year.Add(yearname);
            }
            var yearo = year.OrderByDescending(u => u.city);
            ViewBag.Species = null;
            ViewBag.year = null;
            ViewBag.yearSpecies = Newtonsoft.Json.JsonConvert.SerializeObject(yearo);

            var onlyOnePlant = await GetYearDistAsync("垂柳","2008");
            if(onlyOnePlant != null){
                List<plantResult> plantsData = new List<plantResult>();
                foreach(var item in onlyOnePlant)
                {
                    plantResult plant = new plantResult();
                    plant.datax1 = convertDate(item.叶芽开始膨大期);
                    plant.datax2 = convertDate(item.叶芽开放期);
                    plant.datax3 = convertDate(item.花芽开始膨大期);
                    plant.datax4 = convertDate(item.花芽开放期);
                    plant.datax5 = convertDate(item.开始展叶期);
                    plant.datax6 = convertDate(item.展叶盛期);
                    plant.datax7 = convertDate(item.花序或花蕾出现期);
                    plant.datax8 = convertDate(item.开花始期);
                    plant.datax9 = convertDate(item.开花盛期);
                    plant.datax10 = convertDate(item.开花末期);
                    plant.datax11 = convertDate(item.第二次开花期);
                    plant.datax12 = convertDate(item.果实成熟期);
                    plant.datax13 = convertDate(item.果实脱落开始期);
                    plant.datax14 = convertDate(item.果实脱落末期);
                    plant.datax15 = convertDate(item.叶开始变色期);
                    plant.datax16 = convertDate(item.叶全部变色期);
                    plant.datax17 = convertDate(item.开始落叶期);
                    plant.datax18 = convertDate(item.落叶末期);
                    plant.datay = item.年份+'年'; 
                    plant.species = "垂柳";   
                    plant.city=item.站点;        
                    plantsData.Add(plant);
                }
                ViewBag.onlyPlant = Newtonsoft.Json.JsonConvert.SerializeObject(plantsData);
            }
            return View();
        }

        // POST: /Analysis/stationsAnalysis
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> stationsAnalysis(string species, string year)
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            ViewData["UserName"] = username;

            var Stationslist = await GetStationLocation(species);
            ViewBag.stationsLocation = Newtonsoft.Json.JsonConvert.SerializeObject(Stationslist);

            List<allpoint> citySpecies = new List<allpoint>();          
            citySpecies =_context.allpoint.Where(a => a.植物名 == species).ToList();
            IEnumerable<IGrouping<string, allpoint>> plantl = citySpecies.GroupBy(u => u.年份);
            List<CityResult> yearl = new List<CityResult>();
            foreach(IGrouping<string, allpoint> item in plantl)
            {
                List<allpoint> plantlist = item.ToList();
                CityResult yearname = new CityResult();
                yearname.city = plantlist[0].年份;
                yearl.Add(yearname);
            }
            var yearo = yearl.OrderByDescending(u => u.city);
            ViewBag.Species = species;
            ViewBag.yearSpecies = Newtonsoft.Json.JsonConvert.SerializeObject(yearo);
            ViewBag.year = year;

            var onlyOnePlant = await GetYearDistAsync(species, year);
            if(onlyOnePlant != null){
                List<plantResult> plantsData = new List<plantResult>();
                foreach(var item in onlyOnePlant)
                {
                    plantResult plant = new plantResult();
                    plant.datax1 = convertDate(item.叶芽开始膨大期);
                    plant.datax2 = convertDate(item.叶芽开放期);
                    plant.datax3 = convertDate(item.花芽开始膨大期);
                    plant.datax4 = convertDate(item.花芽开放期);
                    plant.datax5 = convertDate(item.开始展叶期);
                    plant.datax6 = convertDate(item.展叶盛期);
                    plant.datax7 = convertDate(item.花序或花蕾出现期);
                    plant.datax8 = convertDate(item.开花始期);
                    plant.datax9 = convertDate(item.开花盛期);
                    plant.datax10 = convertDate(item.开花末期);
                    plant.datax11 = convertDate(item.第二次开花期);
                    plant.datax12 = convertDate(item.果实成熟期);
                    plant.datax13 = convertDate(item.果实脱落开始期);
                    plant.datax14 = convertDate(item.果实脱落末期);
                    plant.datax15 = convertDate(item.叶开始变色期);
                    plant.datax16 = convertDate(item.叶全部变色期);
                    plant.datax17 = convertDate(item.开始落叶期);
                    plant.datax18 = convertDate(item.落叶末期);
                    plant.datay = item.年份+'年';  
                    plant.species = species; 
                    plant.city=item.站点;        
                    plantsData.Add(plant);
                }
                ViewBag.onlyPlant = Newtonsoft.Json.JsonConvert.SerializeObject(plantsData);
            }
            return View();
        }

        private string convertDate(string date)
        {
            if(date != ""){
                if(date.IndexOf("/") >= 0){
                    date = date.Replace("/","月");
                }
                if(date.IndexOf("日") < 0){
                    date = date + "日";
                }
            }
            return date;
        }

        [HttpPost]
        [AllowAnonymous]
        private Task<List<allpoint>> GetDatasAsync(string city, string species)//获取单一城市单一植物物种的数据
        {
            List<allpoint> onlyOnePlant = new List<allpoint>();          
            onlyOnePlant =_context.allpoint.Where(b => b.植物名 == species && b.站点 == city).ToList();
            return Task.FromResult(onlyOnePlant);//用来创建一个带返回值的、已完成的Task
        }

        private Task<List<allpoint>> GetYearDistAsync(string species, string year)
        {
            List<allpoint> onlyOnePlant = new List<allpoint>();          
            onlyOnePlant =_context.allpoint.Where(u => u.植物名 == species && u.年份 == year).ToList();
            return Task.FromResult(onlyOnePlant);//用来创建一个带返回值的、已完成的Task
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

        //
        // POST: /Analysis/GetSpeciesListAsync
        [HttpPost]
        [AllowAnonymous]
        public string GetSpeciesListAsync([FromBody]CityResult res)//获取单一城市植物物种
        {
            List<allpoint> citySpecies = new List<allpoint>();          
            citySpecies =_context.allpoint.Where(a => a.站点 == res.city).ToList();
            IEnumerable<IGrouping<string, allpoint>> plant = citySpecies.GroupBy(u => u.植物名);
            List<SpieciesResult> species = new List<SpieciesResult>();
            foreach(IGrouping<string, allpoint> item in plant)
            {
                List<allpoint> plantlist = item.ToList();
                SpieciesResult speciesname = new SpieciesResult();
                speciesname.species = plantlist[0].植物名;
                species.Add(speciesname);
            }
            string citySpeciesList = Newtonsoft.Json.JsonConvert.SerializeObject(species);

            return citySpeciesList;
        }

        //
        // POST: /Analysis/GetYearListAsync
        [HttpPost]
        [AllowAnonymous]
        public string GetYearListAsync([FromBody]SpieciesResult spi)
        {
            List<allpoint> citySpecies = new List<allpoint>();          
            citySpecies =_context.allpoint.Where(a => a.植物名 == spi.species).ToList();
            IEnumerable<IGrouping<string, allpoint>> plant = citySpecies.GroupBy(u => u.年份);
            List<CityResult> year = new List<CityResult>();
            foreach(IGrouping<string, allpoint> item in plant)
            {
                List<allpoint> plantlist = item.ToList();
                CityResult yearname = new CityResult();
                yearname.city = plantlist[0].年份;
                year.Add(yearname);
            }
            var yearo = year.OrderByDescending(u => u.city);
            string cityYearList = Newtonsoft.Json.JsonConvert.SerializeObject(yearo);

            return cityYearList;
        }

        //
        // POST: /Analysis/GetSpeciesAsync
        [HttpPost]
        [AllowAnonymous]
        private Task<List<allpoint>> GetSpeciesAsync(string city)//获取单一城市植物物种
        {
            List<allpoint> citySpecies = new List<allpoint>();          
            citySpecies =_context.allpoint.Where(a => a.站点 == city).ToList();         
            return Task.FromResult(citySpecies);//用来创建一个带返回值的、已完成的Task
        }

        [HttpPost]
        [AllowAnonymous]
        private Task<List<allpoint>> GetPlantDistAsync(string species)//获取单一城市单一植物物种的数据
        {
            List<allpoint> onlyOnePlant = new List<allpoint>();          
            onlyOnePlant =_context.allpoint.Where(b => b.植物名 == species).ToList();
            return Task.FromResult(onlyOnePlant);//用来创建一个带返回值的、已完成的Task
        }

        [HttpPost]
        public IActionResult ImportExcel(IFormFile excelfile)
        {
            string sWebRootFolder = _environment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
                {
                    excelfile.CopyTo(fs);
                    fs.Flush();
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    bool bHeaderRow = true;
                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            if (bHeaderRow)
                            {
                                sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
                            }
                            else
                            {
                                sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    return Content(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public class CityResult
        {
            public string city { get; set; }

        }

        public class SpieciesResult
        {
            public string species { get; set; }

        }

        public class stationLocation
        {
            public string  station { get; set; }
            public float  stationLatitude { get; set; }
            public float  stationLongitude{ get; set; }

        }

        public class plantResult
        {
            public string datax1 { get; set; }
            public string datax2 { get; set; }
            public string datax3 { get; set; }
            public string datax4 { get; set; }
            public string datax5 { get; set; }
            public string datax6 { get; set; }
            public string datax7 { get; set; }
            public string datax8 { get; set; }
            public string datax9 { get; set; }
            public string datax10 { get; set; }
            public string datax11 { get; set; }
            public string datax12 { get; set; }
            public string datax13 { get; set; }
            public string datax14 { get; set; }
            public string datax15 { get; set; }
            public string datax16 { get; set; }
            public string datax17 { get; set; }
            public string datax18 { get; set; }
            public string zhuji { get; set; }
            public string datay { get; set; }
            public string species { get; set; }
            public string city { get; set; }
        }
    }
}
