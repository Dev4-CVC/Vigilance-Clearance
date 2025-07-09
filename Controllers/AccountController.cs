using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CVOIS.Services;
using VigilanceClearance.Services;
using VigilanceClearance.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.DTOs;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VigilanceClearance.Interface.Account;
using VigilanceClearance.Models.Modal_Properties.Account;

namespace VigilanceClearance.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CaptchaService _captchaService;
        private readonly IAuthService _authService;

        public AccountController(IHttpContextAccessor httpContextAccessor, CaptchaService captchaService, IAuthService authService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._captchaService = captchaService;
            this._authService = authService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var loginDto = new LoginDto
                {
                    Email = model.Username,
                    Password = model.Password
                };

                var tokenResponse = await _authService.LoginAsync(loginDto);

                if (tokenResponse?.Token == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
                    return View(model);
                }

                HttpContext.Session.SetString("AccessToken", tokenResponse.Token);

               
                HttpContext.Session.SetString("Username", model.Username);
                //return RedirectToAction("PESB_Dashboard", "PESB");

                return RedirectToAction("Index", "Ministry_Department");


            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult GenerateCaptcha()
        {
            string captchaCode;
            byte[] captchaImage = _captchaService.GenerateCaptcha(out captchaCode);
            HttpContext.Session.SetString("CaptchaCode", captchaCode);
            return File(captchaImage, "image/png");
        }

        public bool ValidateCaptcha(string userInput)
        {
            string sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            return sessionCaptcha != null && sessionCaptcha.Equals(userInput, StringComparison.OrdinalIgnoreCase);
        }


        [HttpPost]
        public JsonResult ValidateCaptchaAjax(string captcha)
        {
            bool isValid = ValidateCaptcha(captcha);
            return Json(new { success = isValid });
        }


    }
}
