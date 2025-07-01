using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.Ministry;
using VigilanceClearance.Models.ViewModel.PESB;

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
            string username = "minpower";
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {

               
                var client = _clientFactory.CreateClient();
                //var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");
                // iski jagah dusri API Call Karni As per requrement
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}VcReferenceReceivedFor/VcReferenceReceivedForGetById?UserName={Uri.EscapeDataString(username)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                //var apiResponse = JsonConvert.DeserializeObject<ReferenceReceivedFromCVCAPIResponseModel>(jsonResult);

                //if (apiResponse != null && apiResponse.data != null)
                //{
                //    return View(apiResponse.data); // Pass the list directly to the View
                //}

                var apiResponse = JsonConvert.DeserializeObject<ReferenceReceivedFromCVCAPIResponseModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                    var modelList = new List<ReferenceReceivedFromCVCModel> { apiResponse.data };
                    return View(modelList); // pass as a list to View
                }

                return View(new List<ReferenceReceivedFromCVCModel>()); // empty list fallback

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewOfficerListForGivenProposal(string id)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {

                // iski jagah dusri API Call Karni As per requrement
                var client = _clientFactory.CreateClient();
                //var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");
                return View();
                // iski jagah dusri API Call Karni As per requrement
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                //return View( { CountryList = new List<SelectListItem>() });
                

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}OfficerDetails/OfficerDetailsGetByMasterReferenceID?id={Uri.EscapeDataString(id)}");


                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();
                
                var apiResponse = JsonConvert.DeserializeObject<OfficerListAPIModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                    return View(apiResponse.data); //  Send list directly to view
                }

                return View(new List<OfficerListModel>());

                //return View(new List<OfficerListModel>()); // empty list fallback
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
      

        [HttpGet]
        public async Task<IActionResult> UpdateReferenceReceivedCVC()
        {
            try
            {
                string orgcode = string.Empty;
                //var model = new UpdateReferenceReceivedFromCVCModel
                //{
                //    OrganizationList = await GetOrganizationDropDownAsync("0"),
                //};
                
                var model = new OfficerDetailMainModel
                {
                    officerPostingDetail7 = new OfficerPostingDetails
                    {
                        OrganizationList = await GetOrganizationDropDownAsync("0"),
                        Organization = null // or default
                    }
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



        [HttpGet]
        public async Task<IActionResult> GetMinistryDropDown(string orgcode)
        {
            try
            {
                var item = await GetMinistryDropDownbycodeAsync(orgcode);
                return Json(item);
            }
            catch
            {
                return Json(new { Error = "Failed to load sub-posts." });
            }
        }

        private async Task<List<SelectListItem>> GetMinistryDropDownbycodeAsync(string orgcode)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}DropDown/GetMinistryByOrgCode?orgcode={Uri.EscapeDataString(orgcode)}");

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




        // Insert  API


        [HttpPost]
        public async Task<IActionResult> AddOfficerPostingDetails(OfficerDetailMainModel model)
        {
            InsertOfficerDetailsModel insertOfficerDetailsModel = new InsertOfficerDetailsModel();

            try
            {
                insertOfficerDetailsModel.orgCode = model.officerPostingDetail7.Organization;
                insertOfficerDetailsModel.orgMinistry = model.officerPostingDetail7.Ministry;
                insertOfficerDetailsModel.designation = model.officerPostingDetail7.Designation;
                insertOfficerDetailsModel.placeOfPosting = "Testing";
                insertOfficerDetailsModel.fromDate = model.officerPostingDetail7.TenureFrom;
                insertOfficerDetailsModel.toDate = model.officerPostingDetail7.TenureTo;
                insertOfficerDetailsModel.createdBy = HttpContext.Session.GetString("Username");
                insertOfficerDetailsModel.createdOn = DateTime.Now;
                insertOfficerDetailsModel.createdBySessionId = HttpContext.Session.Id;
                insertOfficerDetailsModel.createdByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                if (ModelState.IsValid)
                {
                    int num = await InsertOfficerPostingDetails(insertOfficerDetailsModel);

                    if(num > 0)
                    {
                        TempData["successmsg"] = "Data Submitted Successfuly";

                        return RedirectToAction("UpdateReferenceReceivedCVC"); // Send the update page
                    }
                }
                //else
                //{
                    TempData["successmsg"] = "Data Not Submitted";

                    return RedirectToAction("UpdateReferenceReceivedCVC");
                //}


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }

            
            //return View(model);

        }



        public async Task<int> InsertOfficerPostingDetails(InsertOfficerDetailsModel objmodel)
        {
            try
            {
                //var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                var accessToken = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}OfficerPostingDetails/addOfficerPostingDetails";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(objmodel, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    return 0;
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                if (int.TryParse(responseJson, out int result))
                    return result;

                // Optionally handle more structured API response if needed
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return 0;
            }
        }


    }
}
