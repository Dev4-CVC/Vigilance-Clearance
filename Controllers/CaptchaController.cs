using Microsoft.AspNetCore.Mvc;
using VigilanceClearance.Services;

namespace VigilanceClearance.Controllers
{
    public class CaptchaController : Controller
    {
        private readonly CaptchaService _captchaService;

        public CaptchaController(CaptchaService captchaService)
        {
            this._captchaService = captchaService;
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
