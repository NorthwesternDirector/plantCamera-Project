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
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _environment;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;
        private ApplicationDbContext  _context;
        private static readonly string _privateKey = @"MIICXQIBAAKBgQDv32K/c7XC9KiCzad5C4l8xEvE7zUOf4//0G9pg7kiZNeJ2OPCD7cJqPMRMwDQbWkwSjq6jIWzM9i9q+D/bLyKr/+rOUfYpIfEXYYCxIogVb/FBnZkVownaHbRlI/WMJj7OXHSWVTWfeTinygUopXyoCFZuO9pkPzo8i5LJds4YQIDAQABAoGAWYiaBTgdag2EMH6uuiGySd3DIljfBvBaQXP9gfbmzY2yXgOUz8Sp33MudwcAkMul7Z5nWWZGKWN6zSEwtGZT4P9S62v7yqiO8HpskjP5w2IZlTqfmWJbp7RWa0mbV/R0HikzKGyllmQ8vuPa7vdOvIMN+ubzprOJnt1wd6aiQRkCQQD9X/3jHd/sRpDG42dgdZhb2JKkU4AFG1YejmshwiFI0O7AiXVkIyC0sNMDWst9MS/bU4XQ13hXRCev/HL3Q3ybAkEA8luVJwlwYBQ7bE2KCK1p5o2kfmzxkUndiHexm/zuJvg5YAypEBbzPVA+EV6zozWgnM0DDZhVSAjkrsSOKRbIswJBALJKbbj3OK5mKVIKip/Rn9hhWS7QovD0/3CY/sHOfOVkP9yz3SsNnOII2zMtHKuHhQlsiGkgdcnSZ6rqlWtwzakCQAcXww+3aQCG1w5eXpHOcXD76DqC+bbk0ITz69DC4D1nulYjSLdd1JSQyqA7g0pdgWvVXCeDs8s952NuATMWpZ0CQQDrVZgZbo/rUSg69wiaSR1RVRrG/YxU5wYvh9ifjSjKCfbC27rexZpGl/NNpDyaxVaKl6Skue7opQGkJbgCY8rf";

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            IHostingEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _environment = environment;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            try{
                var xPassword = DecryptoData(model.Password, _privateKey);
                model.Password = xPassword;
            }catch{
                ModelState.AddModelError("loginError", "密码格式有误");
                return View(model);
            }
            
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                ApplicationUser signinUser = await _userManager.FindByEmailAsync(model.Email);
                if(signinUser != null){
                    var result = await _signInManager.PasswordSignInAsync(signinUser.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, "User logged in.");
                        if (signinUser == null)
                        {
                            ModelState.AddModelError("loginError", "用户不存在");
                            return RedirectToAction("Login", "Account");
                        }else{
                            var lastlogintime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                            signinUser.LoginTimes += 1;
                            signinUser.LastLoginTime = lastlogintime;
                            await _userManager.UpdateAsync(signinUser);
                            return RedirectToAction("Index","Home");
                        }
                    }else{
                        ModelState.AddModelError("loginError", "用户名或密码错误");
                        return View(model);
                    }
                }else{
                    ModelState.AddModelError("loginError", "用户不存在");
                    return View(model);
                }  
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("loginError", "无效的登录");
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(model.InviteCode == "abcd"){
                var createTime = DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss");
                var createUserIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                if(createUserIP == "::1"){
                    createUserIP = "127.0.0.1";
                }
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { 
                        UserName = model.UserName, 
                        Email = model.Email,
                        CreateTime = createTime,
                        CreateUserIP = createUserIP,
                        InviteCode = model.InviteCode,
                        LoginTimes = 1,
                        LastLoginTime = createTime
                        };
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
                                AlbumText = "系统默认相册，不可删除。",
                                AlbumTime = createTime,
                                CoverLink = "../images/album-cover/album-default.png"
                            }
                        );
                        _context.SaveChanges();
                        
                        _logger.LogInformation(3, "User created a new account with password.");
                        return RedirectToAction("Index","Home");
                    }
                    AddErrors(result);
                }
            }else{
                return View(model);
            }

            // 出错了
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                //return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public string DecryptoData(string data, string key){
            RSA rsa = CreateRsaFromPrivateKey(key);
            var dataBytes = System.Convert.FromBase64String(data);
            var userdataBytes = rsa.Decrypt(dataBytes, RSAEncryptionPadding.Pkcs1);
            var userdata = Encoding.UTF8.GetString(userdataBytes);
            return userdata;
        }

        public static string DecryptoData(string data){
            RSA rsa = CreateRsaFromPrivateKey(_privateKey);
            var dataBytes = System.Convert.FromBase64String(data);
            var userdataBytes = rsa.Decrypt(dataBytes, RSAEncryptionPadding.Pkcs1);
            var userdata = Encoding.UTF8.GetString(userdataBytes);
            return userdata;
        }

        private static RSA CreateRsaFromPrivateKey(string privateKey){
            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            var rsa = RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        #endregion
    }
}
