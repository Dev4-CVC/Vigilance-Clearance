using Microsoft.AspNetCore.Mvc;

namespace VigilanceClearance.Controllers
{
    public class CBI_MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
