using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using VigilanceClearance.Models.Entities;
using Microsoft.AspNetCore.Http;
using VigilanceClearance.Models.ViewModel;
using CVOIS.Services;
using VigilanceClearance.Services;

namespace VigilanceClearance.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CaptchaService _captchaService;
        public AccountController(IHttpContextAccessor httpContextAccessor, CaptchaService captchaService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._captchaService = captchaService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    try
        //    {
        //        //captcha validation
        //        var sessionCaptcha = _httpContextAccessor.HttpContext.Session.GetString("CaptchaCode");
        //        if (string.IsNullOrEmpty(sessionCaptcha) || !sessionCaptcha.Equals(model.Captcha, StringComparison.OrdinalIgnoreCase))
        //        {
        //            TempData["ErrorMessage"] = "Invalid Captcha!";
        //            return RedirectToAction("Login", "Home");
        //        }

        //        //fetch user
        //        //var user = _context.loginModel.FirstOrDefault(m => m.UserName == model.Username && m.User_Status == "ACTIVE" && m.ISLOCKEDCNT < 3);

        //        if (user == null)
        //        {
        //            TempData["ErrorMessage"] = "Invalid Username or Account is locked.";
        //            return RedirectToAction("Login", "Home");
        //        }

        //        //password validation
        //        //bool isPasswordValid = SecurityHelper.VerifyPassword1(user.Password, model.Password);
        //        //if (!isPasswordValid)
        //        //{
        //        //    TempData["ErrorMessage"] = "Invalid Password!";
        //        //    return RedirectToAction("Login", "Home");
        //        //}

        //        //set session
        //        _httpContextAccessor.HttpContext.Session.SetString("Username", user.UserName);
        //        _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);
        //        _httpContextAccessor.HttpContext.Session.SetString("Role", user.Role);
        //        _httpContextAccessor.HttpContext.Session.SetString("Fullname", user.Fullname);
        //        _httpContextAccessor.HttpContext.Session.SetString("Mincode", user.Mincode);

        //        //var claims = new List<Claim>
        //        //{
        //        //    new Claim(ClaimTypes.Name, user.UserName),
        //        //    new Claim(ClaimTypes.Role, user.Role)
        //        //};
        //        //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        //var principal = new ClaimsPrincipal(identity);
        //        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //        // Redirect based on role
        //        switch (user.Role.ToUpper())
        //        {
        //            //case "ADMIN":
        //            //    return Redirect("~/Admin/AdminDashboard");
        //            //case "VIEWER":
        //            //    return Redirect("~/Viewer/ViewerDashboard");
        //            //case "SUPERADMIN":
        //            //    return Redirect("~/SuperAdmin/SuperAdminDashboard");
        //            //case "MINISTRY":
        //            //    return Redirect("~/Ministry/MinistryDashboard");
        //            //case "USER":
        //            //    return RedirectToAction("UserDashboard");
        //            default:
        //                return RedirectToAction("AccessDenied");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
        //        return RedirectToAction("Login", "Home");
        //    }
        //}

        [HttpPost]
        public IActionResult Login(LoginViewModel obj)
        {

            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {

            }

            return View();
        }


        [HttpGet]
        public IActionResult GenerateCaptcha()
        {
            string captchaCode;
            byte[] captchaImage = _captchaService.GenerateCaptcha(out captchaCode);

            // Store CAPTCHA in session
            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            return File(captchaImage, "image/png");
        }

        public bool ValidateCaptcha(string userInput)
        {
            string sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            return sessionCaptcha != null && sessionCaptcha.Equals(userInput, StringComparison.OrdinalIgnoreCase);
        }

    }
}
