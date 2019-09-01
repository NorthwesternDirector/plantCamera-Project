using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using plantCamera.Models;




namespace plantCamera.Controllers
{
    [Route("api/[controller]")]
    
    public class IOSLoginController : Controller
    {   
        private readonly SignInManager<ApplicationUser> _signInManager;

        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public IOSLoginController(
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
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


        public async Task<IActionResult> Post(string Email,string password)
        {   
             var backMsgDict = new Dictionary<string,string>();
            if(Email != null && password != null){
            ApplicationUser signinUser = await _userManager.FindByEmailAsync(Email);
            if(signinUser != null)
            {
            var result =  await _signInManager.PasswordSignInAsync(signinUser.UserName,password, false, lockoutOnFailure: false);
            
                if (result.Succeeded)
                {
                // _logger.LogInformation(1, "User logged in.");
                var lastlogintime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                            signinUser.LoginTimes += 1;
                            signinUser.LastLoginTime = lastlogintime;
                            await _userManager.UpdateAsync(signinUser);
                            string username = signinUser.UserName;
                            //backMsgDict.Add("success","登录成功");
                             return Json($"登录成功，用户名:{username}");
                }
            
             else{
                //backMsgDict.Add("failed","用户名或密码错误");
                 return Json("用户名或密码错误");
                 }
            }
            }
            return Json("未注册的用户");
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
