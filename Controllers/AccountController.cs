using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using VigilanceClearance.Models.Entities;
using Microsoft.AspNetCore.Http;
using CVOIS.Services;
using VigilanceClearance.Services;
using VigilanceClearance.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using VigilanceClearance.Models.DTOs;
using System.Linq.Expressions;

namespace VigilanceClearance.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CaptchaService _captchaService;
        private readonly HttpClient _httpClient;

        public AccountController(IHttpContextAccessor httpContextAccessor, CaptchaService captchaService, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _captchaService = captchaService;
            _httpClient = httpClient;
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

                var response = await _httpClient.PostAsJsonAsync("http://10.25.34.185:88/api/Auth/login", loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
                    return View(model);
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

                if (tokenResponse?.Token == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve token. Please try again.");
                    return View(model);
                }

                HttpContext.Session.SetString("AccessToken", tokenResponse.Token);
                return RedirectToAction("PESBDashboard", "PESB");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }

        public class TokenResponse
        {
            [JsonProperty("token")]
            public string Token { get; set; }
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

    }
}
