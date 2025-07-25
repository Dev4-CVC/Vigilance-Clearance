using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using VigilanceClearance.Models.ViewModel.Coord2_DealingHand;
using VigilanceClearance.Models.ViewModel.Comments_Model;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.ViewModel.Ministry;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using VigilanceClearance.Models.ViewModel.CBI_DealingHand;

namespace VigilanceClearance.Controllers
{

    public class Coord2_DealingHandController : Controller
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;
        public Coord2_DealingHandController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration)
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
        public async Task<IActionResult> VcReferenceReceivedDetailsGetAll()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}VcReferenceReceivedFor/VcReferenceReceivedDetailsGetAll");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ReferenceReceivedFromCVCAPIResponseModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                   
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
            HttpContext.Session.Remove("PropsalId");
            HttpContext.Session.SetString("PropsalId", id);

            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}Coord2_DealingHand/OfficerDetailWithPostingDetails?id={Uri.EscapeDataString(id)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ViewOfficerListForGivenProposal>(jsonResult);
                var flatList = apiResponse?.data ?? new List<OfficerDetailsWithPostings>();

                var DataResult = flatList.GroupBy(X => X.Id).ToList();

                var groupedData = flatList
                    .Where(x => x.Id != null)
                    .GroupBy(x => x.Id)
                    .Select(group =>
                    {
                        var first = group.First();

                        return new OfficerAndPostingViewModel
                        {
                            Id = Convert.ToInt32(first.Id),
                            MasterReferenceID = Convert.ToInt32(first.MasterReferenceID),
                            Officer_Name = first.Officer_Name,
                            Officer_FatherName = first.Officer_FatherName,
                            Officer_DateOfBirth = first.Officer_DateOfBirth,
                            Officer_RetirementDate = first.Officer_RetirementDate,
                            Officer_ServiceEntryDate = first.Officer_ServiceEntryDate,
                            Officer_Batch_Year = first.Officer_Batch_Year,
                            Officer_Cadre = first.Officer_Cadre,
                            postingDetailsOfOfficers = group.Where(f => f.Vc_Officer_Id == first.Id).Select(f => new PostingDetailsOfOfficer
                            {

                                orgCode = f.orgCode,
                                designation = f.designation,
                                placeOfPosting = f.placeOfPosting,
                                orgMinistry = f.orgMinistry


                            }).ToList(),
                            cBIFeedbackModels = group.Where(f => f.OfficerIdComments == first.Id).Select(f => new CBIFeedbackModel
                            {

                                id = f.id,
                                FeebbackOf_CBI = f.FeebbackOf_CBI,
                               


                            }).ToList(),
                            vIGFeedbackModels = group.Where(f => f.OfficerIdComments == first.Id).Select(f => new vIGFeedbackModels
                            {

                                id = f.id,

                                vig_Branch_Comments = f.vig_Branch_Comments,



                            }).ToList(),
                            mINFeedbackModels = group.Where(f => f.OfficerIdComments == first.Id).Select(f => new mINFeedbackModels
                            {

                                id = f.id,

                                ministry_Comments = f.ministry_Comments,



                            }).ToList(),
                            CVC_COORD2_DH_COMMENTS = group.Where(f => f.OfficerIdComments == first.Id).Select(f => new DHFeedbackModels
                            {

                                id = f.id,

                                CVC_COORD2_DH_COMMENTS = f.CVC_COORD2_DH_COMMENTS,



                            }).ToList(),
                            CVC_COORD2_SO_COMMENTS = group.Where(f => f.OfficerIdComments == first.Id).Select(f => new SOFeedbackModels
                            {

                                id = f.id,

                                CVC_COORD2_SO_COMMENTS = f.CVC_COORD2_SO_COMMENTS,



                            }).ToList(),
                            CVC_COORD2_BO_COMMENTS = group.Where(f => f.OfficerIdComments == first.Id).Select(f => new BOFeedbackModels
                            {

                                id = f.id,

                                CVC_COORD2_BO_COMMENTS = f.CVC_COORD2_BO_COMMENTS,



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
        public async Task<IActionResult> ViewOfficerListForGivenProposalDHDraft()
        {
            string id = HttpContext.Session.GetString("PropsalId");

            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}OfficerDetails/OfficerDetailsGetByMasterReferenceID?id={Uri.EscapeDataString(id)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<OfficerDetailsDraftViewModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                    return View(apiResponse.data); 
                }

                return View(new List<OfficerDetailsDraftViewModel>());

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateVcReferenceByDH()
        {
            VcReferenceReceivedForUpdate model = new VcReferenceReceivedForUpdate();
            string id = HttpContext.Session.GetString("PropsalId");
            try
            {
                
                model.id = Convert.ToInt32(id);
                model.PendingWith = "CVC_COORD2_BO";
                model.UpdateBy = HttpContext.Session.GetString("Username");
                model.UpdatedBy_SessionId = HttpContext.Session.Id;
                model.UpdatedBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var accessToken = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}VcReferenceReceivedFor/VcReferenceReceivedDetailsUpdate";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(model, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    return Ok(0);
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                if (int.TryParse(responseJson, out int result))
                    return Json(result); 

                return Ok(1); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return Ok(0);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCoord2DealingHandComment(Coord2DealingHandCommentModel model)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is missing.");
            string id = HttpContext.Session.GetString("PropsalId");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                

                   
                    model.OfficerId = model.OfficerId;
                    model.MasterReferenceId = Convert.ToInt32(id);

                    if (!string.IsNullOrWhiteSpace(model.Coord2_DH_comments.AdverseRemarks))
                    {
                        model.Coord2_DH_comments.AdverseRemarks = $"Case: {model.Coord2_DH_comments.AdverseRemarks}";
                    }
                    else
                    {
                        model.Coord2_DH_comments.AdverseRemarks = $"Case: {model.Coord2_DH_comments.AdverseOrNothingAdverse}";
                    }
                    model.CreatedBy_Role = HttpContext.Session.GetString("Username");
                    model.CreatedById = HttpContext.Session.GetString("Username");
                   
                    int result = await SaveDealingHandComment(model); // DB method

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

                    return RedirectToAction("ViewOfficerListForGivenProposal", new { id = ProposalId });


                

                //TempData["errormsg"] = "Invalid form data.";
                return RedirectToAction("ViewOfficerListForGivenProposal");
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = "An error occurred.";
                return RedirectToAction("ViewOfficerListForGivenProposal");
            }
        }

        public async Task<int> SaveDealingHandComment(Coord2DealingHandCommentModel objmodel)
        {
            try
            {
                var accessToken = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}Coord2_DealingHand/RemarksOf_Coord2DH";

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
