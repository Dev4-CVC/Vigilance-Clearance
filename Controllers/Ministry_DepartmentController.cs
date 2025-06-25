using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VigilanceClearance.Models;
using VigilanceClearance.Models.ViewModel;

namespace VigilanceClearance.Controllers
{
    public class Ministry_DepartmentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;

        public Ministry_DepartmentController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration)
        {
            this._clientFactory = clientFactory;
            _httpClient = httpClient;
            _configuration = configuration;
            BaseUrl = _configuration["BaseUrl"]!;
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
        public async Task<IActionResult> UpdateReferenceReceivedCVC( )
        {
            try
            {

                var model = new UpdateReferenceReceivedFromCVCModel
                {
                    OrganizationList = await GetOrganizationDropDownAsync("0"),
                    //MinistryList = await GetMinistryListDropDownAsync(),
                    
                   
                };

                return View(model);

            }
             catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }           
        }

        private async Task<List<SelectListItem>> GetOrganizationDropDownAsync(string section)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}DropDown/MinistryNewGetBySection?section={Uri.EscapeDataString(section)}");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<SelectListItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new();

                return items.Select(item => new SelectListItem
                {
                    Value = item.Value,
                    Text = item.Text
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

    }
}
