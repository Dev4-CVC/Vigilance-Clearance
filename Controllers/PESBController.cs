using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VigilanceClearance.Controllers
{
    public class PESBController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PESBController(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }
        public IActionResult Index()
        {
            PESBViewModel model = new PESBViewModel();  
            model.PostList = new List<SelectListItem>()
            {
                new SelectListItem { Text= "Chairman", Value="Chairman" },
                new SelectListItem { Text = "Chairman & Managing Director", Value = "CMD" },
                new SelectListItem { Text = "Director", Value = "Director" }
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CountryData()
        {
            var client = _clientFactory.CreateClient();

            try
            {
                var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");

                var model = new CountryViewModel
                {
                    CountryList = countries.OrderBy(c => c.Name.Common).Select(c => new SelectListItem
                    {
                        Text = c.Name.Common,
                        Value = c.Cca2
                    }).ToList()
                };
                return View(model);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View(new CountryViewModel { CountryList = new List<SelectListItem>() });
            }
        }

        [HttpPost]
        public IActionResult Index(PESBViewModel model)
        {
            model.PostList = new List<SelectListItem>
            {
                new SelectListItem { Text= "Chairman", Value="Chairman" },
                new SelectListItem { Text = "Chairman & Managing Director", Value = "CMD" },
                new SelectListItem { Text = "Director", Value = "Director" }
            };

            if (ModelState.IsValid)
            {
                // Process and redirect or show success
            }

            return View(model);
        }


    }
}
