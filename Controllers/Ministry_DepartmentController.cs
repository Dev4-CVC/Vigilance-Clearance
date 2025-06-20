using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.ViewModel;

namespace VigilanceClearance.Controllers
{
    public class Ministry_DepartmentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Ministry_DepartmentController(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewReferenceReceivedCVC()
        {
            try
            {
                // iski jagah dusri API Call Karni As per requrement
                var client = _clientFactory.CreateClient();
                var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");
                // iski jagah dusri API Call Karni As per requrement
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> viewDetailsReferenceReceivedCVC()
        {
            try
            {
                // iski jagah dusri API Call Karni As per requrement
                var client = _clientFactory.CreateClient();
                var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");

                // iski jagah dusri API Call Karni As per requrement
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View(new CountryViewModel { CountryList = new List<SelectListItem>() });

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateReferenceReceivedCVC()
        {
            try
            {
                // iski jagah dusri API Call Karni As per requrement
                var client = _clientFactory.CreateClient();
                var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");

                var model = new UpdateReferenceReceivedFromCVCModel
                {
                    CountryList = countries.OrderBy(c => c.Name.Common).Select(c => new SelectListItem
                    {
                        Text = c.Name.Common,
                        Value = c.Cca2
                    }).ToList()
                };

                return View(model);
                // iski jagah dusri API Call Karni As per requrement
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View(new CountryViewModel { CountryList = new List<SelectListItem>() });
            }
            //return View();
        }
    }
}
