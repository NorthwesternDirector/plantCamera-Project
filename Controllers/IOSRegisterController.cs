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
using plantCamera.Services;




namespace plantCamera.Controllers
{
    [Route("api/[controller]")]
    
    public class IOSRegisterController : Controller
    {   
        private readonly SignInManager<ApplicationUser> _signInManager;

        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        private IHostingEnvironment _environment;

        private ApplicationDbContext  _context;
        public IOSRegisterController(
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment environment,
            ApplicationDbContext context
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
             _environment = environment;
             _context = context;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "webApi", "test" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "userId - " + id;
        }

        //POST api/values
        [HttpPost] 
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]


        public async Task<IActionResult> Post(string username,string email,string password,string inviteCode)
        {   
            if(username != null && email != null && password != null && inviteCode != null){
            if(inviteCode == "abcd"){
                var createTime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                    string createUserIP = "127.0.0.1";
                    var user = new ApplicationUser { 
                        UserName = username, 
                        Email = email,
                        CreateTime = createTime,
                        CreateUserIP = createUserIP,
                        InviteCode = inviteCode,
                        LoginTimes = 1,
                        LastLoginTime = createTime
                        };
                        //return Json($"{username},{email},{password},{inviteCode}");
                        RegisterViewModel model = new RegisterViewModel();
                        model.Password = password;
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        
                        //邮箱验证
                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                        //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        //建立用户文件夹
                        var userId = user.Id;
                        string docPath = _environment.ContentRootPath + "/wwwroot/user/" + userId;
                        if(!Directory.Exists(docPath))
                        {
                            Directory.CreateDirectory(docPath);
                        }
                        //初始相册
                        _context.AlbumB.Add(
                            new AlbumB
                            {
                                Id = userId,
                                AlbumsName = "默认相册",
                                AlbumText = "系统默认相册，不可删除。"
                            }
                        );
                        _context.SaveChanges();
                        
                        return Json("注册成功");
                    }
                    return Json("注册失败");
                
            }else{
               return Json("邀请码错误");
            }
            }
            return Json("注册信息不足");

            // 出错了
            
        }

        

        

        // public async Task<string> Post([FromForm]IOSLogin information)
        // {
            
        //     var passwordx = AccountController.DecryptoData(information.password);
        //     ApplicationUser signinUser = await _userManager.FindByEmailAsync(information.name);
        //     if(signinUser != null){
        //     var result =  await _signInManager.PasswordSignInAsync(information.name, passwordx, false, lockoutOnFailure: false);
        //     if (result.Succeeded){
        //         _logger.LogInformation(1, "User logged in.");
        //         var lastlogintime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
        //                     signinUser.LoginTimes += 1;
        //                     signinUser.LastLoginTime = lastlogintime;
        //                     await _userManager.UpdateAsync(signinUser);
        //                     return "success";
        //     }
        //     else{
        //         return "failed";
        //     }
            
        //     }
        //     return "failed";
            
           

        // }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody]string value)
        // {
        // }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
    }
}
