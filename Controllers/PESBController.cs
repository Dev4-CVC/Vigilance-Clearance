using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mono.TextTemplating;

namespace VigilanceClearance.Controllers
{
    public class PESBController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PESBController(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public IActionResult PESBDashboard()
        {
            try
            {
                ViewBag.title = "PESB Dashboard";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
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

    }
}
