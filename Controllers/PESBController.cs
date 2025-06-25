using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mono.TextTemplating;
using System;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging.Signing;
using System.Runtime.InteropServices;
using Humanizer;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VigilanceClearance.Models;
using System.Net.Http.Headers;
using System.IO;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Controllers
{
    public class PESBController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string APIURL;
        public PESBController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration)
        {
            this._clientFactory = clientFactory;
            _httpClient = httpClient;
            _configuration = configuration;
            APIURL = _configuration["APIURL"]!;
        }


        public async Task<IActionResult> PESB_Dashboard()
        {
            try
            {
                ViewBag.title = "PESB Dashboard";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }

        [HttpGet]
        public async Task<IActionResult> PESBReports()
        {
            try
            {
                ViewBag.title = "PESB : Add New Reference";

                var model = new PESBViewModel
                {
                    PostDescriptionList = await GetPostDescriptionsDropDownAsync("APPOINTMENT"),
                    SubPostDescriptionList = await GetSubPostsByPostCodeInternal(""),
                    ServiceList = await GetService(),
                    BatchList = await GetBatch(),
                    CadreList = await GetCadre()
                };

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


        public async Task<IActionResult> PESB_Appointment()
        {
            try
            {
                ViewBag.title = "PESB Appointment";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }

        [HttpGet]
        public async Task<IActionResult> PESB_Add_New_Reference()
        {
            try
            {
                ViewBag.title = "PESB : Add New Reference";
                var ReferenceCode = "APPOINTMENT";
                var model = new PESB_Add_New_Reference_ViewModel
                {
                    ReferenceDescList = await GetReferenceDropDownAsync(),
                    PostDescriptionList = await GetPostDescriptionsDropDownAsync(ReferenceCode),
                    SubPostDescriptionList= new List<SelectListItem>(),
                    ReferenceCode = "APPOINTMENT"
                };

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }



        //db table name: tbl_Master_Vc_Reference
        private async Task<List<SelectListItem>> GetReferenceDropDownAsync()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}DropDown/VcReferenceGetAll");

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


        //db table name: tbl_Master_Vc_Post
        private async Task<List<SelectListItem>> GetPostDescriptionsDropDownAsync(string ReferenceCode)
        {
            if (string.IsNullOrWhiteSpace(ReferenceCode)) return new List<SelectListItem>();

            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}DropDown/VcPostGetByReferenceNumber?ReferenceNumber={Uri.EscapeDataString(ReferenceCode)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<SelectListItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new List<DropDownResponseModel>();

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
        public async Task<IActionResult> GetSubPostsByPostCode(string postCode)
         {
            try
            {
                var subPosts = await GetSubPostsByPostCodeInternal(postCode);
                return Json(subPosts);
            }
            catch
            {
                return Json(new { Error = "Failed to load sub-posts." });
            }
        }

        //db table name: tbl_Master_Vc_Post_SubCategory
        private async Task<List<SelectListItem>> GetSubPostsByPostCodeInternal(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode)) return new List<SelectListItem>();

            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}DropDown/VcPostSubCategoryGetById?Id={Uri.EscapeDataString(postCode)}";

                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new List<SelectListItem>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(jsonContent) ?? new List<DropDownResponseModel>();

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

       










        private Task<List<SelectListItem>> GetService()
        {
            try
            {
                return Task.FromResult(new List<SelectListItem>
                {
                    new SelectListItem { Value = "VC101", Text = "Vigilance Clearance OFFICER" },
                    new SelectListItem { Value = "CHAIRMAN", Text = "CHAIRMAN" },
                    new SelectListItem { Value = "DIRECTOR", Text = "DIRECTOR" },
                    new SelectListItem { Value = "CRO", Text = "CHIEF RESEARCH Officer" },
                    new SelectListItem { Value = "IAS", Text = "IAS" },
                    new SelectListItem { Value = "IPS", Text = "IPS" },
                    new SelectListItem { Value = "IFoS", Text = "IFoS" },
                    new SelectListItem { Value = "Other", Text = "Other" }
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new List<SelectListItem>());
            }
        }

        private Task<List<SelectListItem>> GetBatch()
        {
            try
            {
                return Task.FromResult(new List<SelectListItem>
                {
                    new SelectListItem { Value = "2001", Text = "2001" },
                    new SelectListItem { Value = "2002", Text = "2002" },
                    new SelectListItem { Value = "2003", Text = "2003" },
                    new SelectListItem { Value = "2004", Text = "2004" },
                    new SelectListItem { Value = "2005", Text = "2005" }
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new List<SelectListItem>());
            }
        }

        private Task<List<SelectListItem>> GetCadre()
        {
            try
            {
                return Task.FromResult(new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Andhra Pradesh" },
                    new SelectListItem { Value = "2", Text = "AGMUT" },
                    new SelectListItem { Value = "3", Text = "Assam_Meghalaya" },
                    new SelectListItem { Value = "4", Text = "Bihar" },
                    new SelectListItem { Value = "5", Text = "Chhattisgarh" }
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new List<SelectListItem>());
            }
        }






        [HttpPost]
        public async Task<IActionResult> PESBReports(PESBViewModel model)
        {
            ViewBag.title = "PESB Report Form";
            try
            {
                if (model.SubPostCode == "Other" && string.IsNullOrWhiteSpace(model.OtherSubPost))
                {
                    ModelState.AddModelError("OtherSubPost", "Please enter sub post.");
                }
                if (model.Service == "Other" && string.IsNullOrWhiteSpace(model.OtherService))
                {
                    ModelState.AddModelError("OtherService", "Please enter service.");
                }


                if (!ModelState.IsValid)
                {
                    model.PostDescriptionList = await GetPostDescriptionsDropDownAsync("APPOINTMENT");
                    model.SubPostDescriptionList = await GetSubPostsByPostCodeInternal("");
                    model.ServiceList = await GetService();
                    model.BatchList = await GetBatch();
                    model.CadreList = await GetCadre();
                    return View(model);
                }
                return RedirectToAction("PESBDashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                model.PostDescriptionList = await GetPostDescriptionsDropDownAsync("APPOINTMENT");
                model.SubPostDescriptionList = await GetSubPostsByPostCodeInternal("");
                return View(model);
            }
        }


    }
}













//private async Task<List<SelectListItem>> GetReferenceDropDownAsync()
//{
//    var accessToken = HttpContext.Session.GetString("AccessToken");
//    if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

//    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

//    var response = await _httpClient.GetAsync(APIURL + "DropDown/VcReferenceGetAll");

//    var json = await response.Content.ReadAsStringAsync();
//    var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new();

//    var selectList = items.Select(item => new SelectListItem
//    {
//        Value = item.Value,
//        Text = item.Text
//    }).ToList();

//    return selectList;
//}

//private async Task<List<SelectListItem>> GetPostDescriptionsDropDownAsync(string id)
//{
//    var accessToken = HttpContext.Session.GetString("AccessToken");
//    _httpClient.DefaultRequestHeaders.Authorization =
//        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

//    var response = await _httpClient.GetAsync(APIURL + "DropDown/VcPostGetByReferenceNumber?ReferenceNumber=APPOINTMENT");

//    if (!response.IsSuccessStatusCode)
//        return new List<SelectListItem>(); // Or handle error accordingly

//    var json = await response.Content.ReadAsStringAsync();
//    var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json);

//    var selectList = items.Select(item => new SelectListItem
//    {
//        Value = item.Value,
//        Text = item.Text
//    }).ToList();

//    return selectList;
//}

// private Task<List<SelectListItem>> GetSubPostsByPostCodeInternal(string postCode)
//{
//    var subPostMap = new Dictionary<string, List<SelectListItem>>(StringComparer.OrdinalIgnoreCase)
//    {
//        ["DIRECTOR"] = new List<SelectListItem>
//        {
//            new SelectListItem { Value = "TECH", Text = "TECHNICAL" },
//            new SelectListItem { Value = "HR", Text = "HR" },
//            new SelectListItem { Value = "OPERATIONS", Text = "OPERATIONS" },
//            new SelectListItem { Value = "COMMERCIAL", Text = "COMMERCIAL" },
//            new SelectListItem { Value = "MARKETING", Text = "MARKETING" },
//            new SelectListItem { Value = "BD", Text = "BUSINESS DEVELOPMENT" },
//            new SelectListItem { Value = "GREENENERGY", Text = "GREEN ENERGY" },
//            new SelectListItem { Value = "Other", Text = "Other" },
//        }
//    };

//    if (!string.IsNullOrWhiteSpace(postCode) && subPostMap.TryGetValue(postCode, out var result))
//        return Task.FromResult(result);

//    return Task.FromResult(new List<SelectListItem>());
//}
