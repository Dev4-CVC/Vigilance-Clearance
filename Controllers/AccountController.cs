using CVOIS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NuGet.Common;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using VigilanceClearance.Interface.Account;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.DTOs;
using VigilanceClearance.Models.Modal_Properties.Account;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Services;
using System.Text.Json;

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
        [ValidateAntiForgeryToken]
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

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tokenResponse.Token);
                // Typical claim for username: "sub" or "unique_name"
                var username = jwtToken.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value
                            ?? jwtToken.Claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;

                // Role claims are usually named "role" or "roles"
                var roles = jwtToken.Claims
                    .Where(c => c.Type.Contains("role") || c.Type.Contains("role"))
                    .Select(c => c.Value)
                    .ToArray();

                HttpContext.Session.SetString("AccessToken", tokenResponse.Token);
                HttpContext.Session.SetString("Username", model.Username);
                var UserDetails = await _authService.GetUserDetailsbyUserName(username);

                // Serialize to JSON
                var userDetailsJson = System.Text.Json.JsonSerializer.Serialize(UserDetails);

                HttpContext.Session.SetString("UserDetails", userDetailsJson);
                HttpContext.Session.SetString("UserRole", roles[0].ToString());
                HttpContext.Session.SetString("Username", username);

                if (roles[0].ToString() == "ROLE_DH" || roles.Contains("ROLE_DH"))
                {
                    var userdetails_Json = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _section = userdetails_Json.ID.ToString();
                    HttpContext.Session.SetString("Section", _section);
                    return RedirectToAction("DH_Dashboard", "DealingHand");
                }

                if (roles.Contains("MINISTRY_DH"))
                {
                    return RedirectToAction("Index", "Ministry_Department");
                }
               else if (roles.Contains("MINISTRY_APPROVER"))
                {
                    return RedirectToAction("Index", "Ministry_Department");
                }
                else if (roles.Contains("MINISTRY_CVO"))
                {
                    return RedirectToAction("Index", "Ministry_Department");
                }
                else if (roles.Contains("User"))
                {
                    return RedirectToAction("PESB_Dashboard", "PESB");
                }
                else if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Users", "Admin");
                }
                else if (roles.Contains("Coor"))
                {
                    return RedirectToAction("Users", "Admin");
                }
                //COORD2 starts 
                else if (roles.Contains("CVC_COORD2_DH"))
                {
                    return RedirectToAction("Index", "Coord2_DealingHand");
                }
                else if (roles.Contains("CVC_COORD2_SO"))
                {
                    return RedirectToAction("Index", "Coord2_BranchOfficer");
                }
                else if (roles.Contains("CVC_COORD2_BO"))
                {
                    return RedirectToAction("Index", "Coord2_BranchOfficer");
                }
                //COORD2 ends

                else if (roles.Contains("CBI_DH"))
                {
                    return RedirectToAction("Index", "CBI_DealingHand");
                }
                else if (roles.Contains("CBI_APPROVER"))
                {
                    return RedirectToAction("Index", "CBI_DealingHand");
                }
                else if (roles.Contains("CBI_CA"))
                {
                    return RedirectToAction("Index", "CBI_DealingHand");
                }
                else if (roles.Contains("ROLE_DH"))
                {
                    return RedirectToAction("DH_Dashboard", "DealingHand");
                }
                else if (roles.Contains("ROLE_SO"))
                {
                    return RedirectToAction("SO_Dashboard", "SectionOfficer");
                }
                else if (roles.Contains("ROLE_BO"))
                {
                    return RedirectToAction("BO_Dashboard", "BranchOfficer");
                }

                return View(model);
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


        [HttpGet]
        public async Task<IActionResult> LogoutRedirect()
        {
            // Clear authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session
            HttpContext.Session.Clear();

            // Optional: Clear TempData
            TempData.Clear();

            // Optional: Redirect to Login or Home
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clear authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear session
            HttpContext.Session.Clear();

            // Optional: Clear TempData
            TempData.Clear();

            // Optional: Redirect to Login or Home
            return RedirectToAction("Login", "Account");
        }

        

    }
}
