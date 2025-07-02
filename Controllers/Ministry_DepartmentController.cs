using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models;
using VigilanceClearance.Models.DTOs;
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
        private readonly IMinistry _ministry;

        public Ministry_DepartmentController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration, IMinistry ministry)
        {
            this._clientFactory = clientFactory;
            _httpClient = httpClient;
            _configuration = configuration;
            BaseUrl = _configuration["BaseUrl"]!;
            this._ministry = ministry;
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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}OfficerDetails/OfficerDetailsGetByMasterReferenceID?id={Uri.EscapeDataString(id)}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

                var jsonResult = await response.Content.ReadAsStringAsync();
                
                var apiResponse = JsonConvert.DeserializeObject<OfficerListAPIModel>(jsonResult);

                if (apiResponse != null && apiResponse.data != null)
                {
                    List<OfficerListModel> obj = apiResponse.data;
                    HttpContext.Session.SetString("apiResponse", JsonConvert.SerializeObject(obj));

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
      

        [HttpPost]
        public async Task<IActionResult> UpdateReferenceReceivedCVC([FromBody] OfficerListModel data)
        {
            try
            {
                //List<OfficerListModel> result = new List<OfficerListModel>();

                //var json = HttpContext.Session.GetString("apiResponse");
                //if (json != null)
                //{
                //    result = JsonConvert.DeserializeObject<List<OfficerListModel>>(json);
                //}

                //var newdata = result.Find(x => x.Id == int.Parse(id));
               
                
                string orgcode = string.Empty;
               
                //var data = await ViewOfficerListForGivenProposal(id);

                var model = new OfficerDetailMainModel
                {
                    officerPostingDetail7 = new OfficerPostingDetails
                    {
                        /* OrganizationList = await GetOrganizationDropDownAsync("0")*/
                        OrganizationList = await _ministry.GetOrganizationDropDownAsync("0"),
                        Organization = null // or default
                    },

                    //officerPersonalDetailModel = new OfficerPersonalDetailModel
                    //{
                    //    Officer_Name = newdata.Officer_Name,
                    //    Officer_FatherName = newdata.Officer_FatherName,
                    //    Officer_DateOfBirth = newdata.Officer_DateOfBirth.ToString("dd-MM-yyyy"),
                    //    Officer_RetirementDate = newdata.Officer_RetirementDate.ToString("dd-MM-yyyy"),
                    //    Officer_ServiceEntryDate= newdata.Officer_ServiceEntryDate.ToString("dd-MM-yyyy"),
                    //    Officer_Service = newdata.Officer_Service,
                    //    Officer_Batch_Year = newdata.Officer_Batch_Year.ToString(),
                    //    Cadre = newdata.Officer_Cadre

                    //}
                };

                return View(model);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }

        //private async Task<List<SelectListItem>> GetOrganizationDropDownAsync(string section)
        //{
        //    var accessToken = HttpContext.Session.GetString("AccessToken");
        //    if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        var response = await _httpClient.GetAsync($"{BaseUrl}DropDown/MinistryNewGetBySection?section={Uri.EscapeDataString(section)}");

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new List<SelectListItem>();
        //        }

        //        var json = await response.Content.ReadAsStringAsync();
        //        var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new();

        //        return items.Select(item => new SelectListItem
        //        {
        //            Value = item.Value,
        //            Text = item.Text
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<SelectListItem>();
        //    }
        //}



        [HttpGet]
        public async Task<IActionResult> GetMinistryDropDown(string orgcode)
        {
            try
            {
                //var item = await GetMinistryDropDownbycodeAsync(orgcode);
                var item = await _ministry.GetMinistryDropDownbycodeAsync(orgcode);
                return Json(item);
            }
            catch
            {
                return Json(new { Error = "Failed to load sub-posts." });
            }
        }

        //private async Task<List<SelectListItem>> GetMinistryDropDownbycodeAsync(string orgcode)
        //{
        //    var accessToken = HttpContext.Session.GetString("AccessToken");
        //    if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        var response = await _httpClient.GetAsync($"{BaseUrl}DropDown/GetMinistryByOrgCode?orgcode={Uri.EscapeDataString(orgcode)}");

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new List<SelectListItem>();
        //        }

        //        var json = await response.Content.ReadAsStringAsync();
        //        var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new();

        //        return items.Select(item => new SelectListItem
        //        {
        //            Value = item.Value,
        //            Text = item.Text
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<SelectListItem>();
        //    }
        //}




        // Insert  API


        [HttpPost]
        public async Task<IActionResult> AddOfficerPostingDetails(OfficerDetailMainModel model)
        {
            InsertOfficerDetailsModel insertOfficerDetailsModel = new InsertOfficerDetailsModel();

            try
            {
                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerPostingDetail7, nameof(model.officerPostingDetail7)))
                {
                    // Submodel is valid!
                    insertOfficerDetailsModel.OrgCode = model.officerPostingDetail7.Organization;
                    insertOfficerDetailsModel.OrgMinistry = model.officerPostingDetail7.Ministry;
                    insertOfficerDetailsModel.Designation = model.officerPostingDetail7.Designation;
                    insertOfficerDetailsModel.PlaceOfPosting = model.officerPostingDetail7.PlaceOfPosting;
                    insertOfficerDetailsModel.VcOfficerId = 212;
                    string fromDate = DateTime.Parse(model.officerPostingDetail7.TenureFrom.ToString()).ToString("dd/MM/yyyy");
                    string toDate = DateTime.Parse(model.officerPostingDetail7.TenureTo.ToString()).ToString("dd/MM/yyyy");

                    insertOfficerDetailsModel.FromDate = fromDate;
                    insertOfficerDetailsModel.ToDate = toDate;
                    insertOfficerDetailsModel.CreatedBy = HttpContext.Session.GetString("Username");

                    insertOfficerDetailsModel.CreatedBySessionId = HttpContext.Session.Id;
                    insertOfficerDetailsModel.CreatedByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                    
                    int num = await _ministry.InsertOfficerPostingDetail(insertOfficerDetailsModel);                  

                    if (num > 0)
                    {
                        TempData["successmsg"] = "Data Submitted Successfuly";
                        TempData.Keep("successmsg");
                    }
                    else
                    {
                        TempData["successmsg"] = "Data Not Submitted";
                    }

               
                    return RedirectToAction("UpdateReferenceReceivedCVC"); // Send the update page
                }

                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC");
                }
                   
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }

        }

    }
}
