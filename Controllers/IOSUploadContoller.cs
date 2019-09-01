using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using plantCamera.Models.DataViewModels;
using plantCamera.Data;
using plantCamera.Models.IOSModels;
using Microsoft.AspNetCore.Identity;
using plantCamera.Models;

namespace plantCamera.Controllers
{
    [Consumes("multipart/form-data")]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class IOSUploadController : Controller
    {   
        private IHostingEnvironment _environment;

        private readonly UserManager<ApplicationUser> _userManager;

        private ApplicationDbContext  _context;

        

        private string _photoUrl;
        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif","ico", "PNG", "JPG", "JPEG", "BMP", "GIF","ICO" };
        public IOSUploadController(
            IHostingEnvironment environment, ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "webApi", "upload" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "userId - " + id;
        }

        //POST api/values
        [HttpPost]
        
         public async Task<IActionResult> Post(string LoginEmail,int source)
        {   if(LoginEmail.Length > 0){
            ApplicationUser signinUser = await _userManager.FindByEmailAsync(LoginEmail);
            string userId = signinUser.Id;
            var uploadFile = Request.Form.Files;
            if (uploadFile.Count > 0)
            {
                string filePath = _environment.ContentRootPath + "/wwwroot/user/" + userId; 
             if(!Directory.Exists(filePath))
             {
                Directory.CreateDirectory(filePath);
             }
                foreach(var file in uploadFile)
                {
                var filename = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string imagePath = filePath + "/" + filename;
                 using(FileStream fs = System.IO.File.Create(imagePath))
                 {
                      await file.CopyToAsync(fs);
                      fs.Flush();
                 }

                
                string extension = filename.Split('.')[1];
                 if(!pictureFormatArray.Contains(extension))
                 {
                    //数据文件
                    string xmlPath = "../../user/" + userId + "/" + filename;
                    string imagePathInDataBase = "../../user/" + userId + "/" + filename.Split('.')[0] + ".JPG";
                    XDocument xmlDoc = XDocument.Load(imagePath);
                    PList plist = new PList();
                    plist.Load(imagePath);
                    var altitude = plist.GetValue("altitude");
                    var longitutde = plist.GetValue("longitude");
                    var latitude = plist.GetValue("latitude");
                    var plantName = plist.GetValue("name");
                    var phenomenon = plist.GetValue("phenomenon");
                    var time = plist.GetValue("time");
                    var weather = plist.GetValue("weather");
                    var wind = plist.GetValue("wind");
                    var date = plist.GetValue("normalDate");
                    var chineseDate = plist.GetValue("chineseDate");
                    
                    if(altitude.ToString() == "未获取"){
                        altitude = "0";
                    }
                    if(latitude.ToString() == "未获取"){
                        latitude = "0";
                    }
                    if(longitutde.ToString() == "未获取"){
                        longitutde = "0";
                    }
                    if(weather.ToString() == "未获取天气"){
                        
                    }
                    else{
                    var weatherArray = weather.ToString().Split('℃');
                    string temperature = weatherArray[0] + "℃" + weatherArray[1] + "℃";
                    weather = weatherArray[2]; 
                    
                    }
                    if(wind.ToString() == "未获取风速"){
                        wind = null;
                    }
                    //var user = _httpContextAccessor.HttpContext.User;
                    //var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var uploadTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                    //var season = Determineseason(model.PhotoTakeTime);
                      PhotoInformation model = new PhotoInformation();
                      model.Id = userId;
                      model.PhotoUploadTime = uploadTime;
                      model.DeleteFlag = false;
                      model.PhotoGrade = 100;
                      model.AlbumName = "默认相册";
                      if(source == 1){
                      model.PhotoSource = "iOS客户端";
                      }
                      else if (source == 2){
                      model.PhotoSource = "Android客户端";
                      }
                      model.PhotoTakeTime = date.ToString() + " " + time.ToString();
                      model.PhotoLatitude = Convert.ToDecimal(latitude.ToString());
                      model.PhotoLongitude = Convert.ToDecimal(longitutde.ToString());
                      model.PhotoHeight = Convert.ToSingle(altitude.ToString());
                      model.PhotoPhenology = phenomenon.ToString();
                      model.Weather = weather.ToString();
                      model.LunarTime = chineseDate.ToString();
                      model.PhotoLink = imagePathInDataBase;
                      model.PhotoPlant = plantName.ToString();
                      //model.Temperature = temperature;
                      //model.Season = season;

                      _context.PhotoInformation.Add(model);
                      _context.SaveChanges();
                 }
                 else{
                 _photoUrl = imagePath;
                 }
                }
                string message = $"{uploadFile.Count} file(s) uploaded successfully!";
                
                    return Json(message);
        
            }
        
            return Json($"{uploadFile.Count} failed uplaod");
            
        }
        else{
            return Json("no account active");
        }
        }
    }
}


