using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using VigilanceClearance.Models.ViewModel.CBI_DealingHand;
using VigilanceClearance.Models.ViewModel.Ministry;

namespace VigilanceClearance.Controllers
{
    public class Cbi_DealingHandController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;

        public Cbi_DealingHandController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration)
        {

            _clientFactory = clientFactory;
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
            string username = "CBI_DH";
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}VcReferenceReceivedFor/VcReferenceReceivedForGetById?UserName={Uri.EscapeDataString(username)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ReferenceReceivedFromCVCAPIResponseModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                    //var modelList = new List<ReferenceReceivedFromCVCModel> { apiResponse.data };
                    return View(apiResponse.data); 
                }

                return View(new List<ReferenceReceivedFromCVCModel>()); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewOfficerListForGivenProposal(string id)
        {
            HttpContext.Session.SetString("PropsalId", id);

            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}CBI_DealingHand/OfficerDetailWithFeedbackOf_CBI?id={Uri.EscapeDataString(id)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<OfficerPersonalDetailModel>(jsonResult);
                var flatList = apiResponse?.data ?? new List<OfficerDetailWithFeedbackModel>();

                var DataResult = flatList.GroupBy(X=>X.Id).ToList();

                var groupedData = flatList
                    .Where(x => x.Id != null)
                    .GroupBy(x => x.Id)
                    .Select(group =>
                    {
                        var first = group.First();

                        return new OfficerAndFeedbackViewModel
                        {
                            Id = first.Id,
                            MasterReferenceID = first.MasterReferenceID,
                            Officer_Name = first.Officer_Name,
                            Officer_FatherName = first.Officer_FatherName,
                            Officer_DateOfBirth = first.Officer_DateOfBirth,
                            Officer_RetirementDate = first.Officer_RetirementDate,
                            Officer_ServiceEntryDate = first.Officer_ServiceEntryDate,
                            Officer_Batch_Year = first.Officer_Batch_Year,
                            Officer_Cadre = first.Officer_Cadre,
                            cBIFeedbackModels = group.Where(f => f.officerId==first.Id).Select(f => new CBIFeedbackModel
                            {
                                
                                FeebbackOf_CBI = f.FeebbackOf_CBI
                                
                            }).ToList()
                        };
                    }).ToList();

                return View(groupedData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FeebbackOfCBIGetById(string id)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}CBI_DealingHand/FeedbackOfCBIGetById?id={Uri.EscapeDataString(id)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<Models.ViewModel.Ministry.OfficerPersonalDetailModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                    return View(apiResponse.data);
                }

                return View(new List<OfficerListModel>());

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCBIAdverseComment(CBIAdverseCommentViewModel model)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var _MasterReferenceId = HttpContext.Session.GetString("PropsalId");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");
           
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                if (ModelState.IsValid)
                {
                    model.MasterReferenceId = Convert.ToInt32(_MasterReferenceId);
                    model.OfficerId = model.OfficerId;
                    model.FeebbackOfCbi = $"Case: {model.FeebbackOfCbi}";
                    model.CreatedBy = HttpContext.Session.GetString("Username");
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBySessionId = HttpContext.Session.Id;
                    model.CreatedByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                    model.UpdateBy = model.CreatedBy;
                    model.UpdatedOn = DateTime.Now;
                    model.UpdatedBySessionId = model.CreatedByIp;
                    model.UpdatedByIp = model.CreatedByIp;

                    int result = await SaveCBIAdverseCommentDetails(model); // DB method

                    if (result > 0)
                    {
                        TempData["successmsg"] = "Data submitted successfully.";
                    }
                    else
                    {
                        TempData["errormsg"] = "Data submission failed.";
                    }

                    var ProposalId = HttpContext.Session.GetString("PropsalId");

                    if (string.IsNullOrWhiteSpace(ProposalId))
                    {
                        return BadRequest("ProposalId not found in session.");
                    }

                    return RedirectToAction("ViewOfficerListForGivenProposal", new { id= ProposalId });


                }

                TempData["errormsg"] = "Invalid form data.";
                return RedirectToAction("ViewOfficerListForGivenProposal");
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = "An error occurred.";
                return RedirectToAction("ViewOfficerListForGivenProposal");
            }
        }

        public async Task<int> SaveCBIAdverseCommentDetails(CBIAdverseCommentViewModel objmodel)
        {
            try
            {
                var accessToken = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}CBI_DealingHand/FeedbackOf_CBI";

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
