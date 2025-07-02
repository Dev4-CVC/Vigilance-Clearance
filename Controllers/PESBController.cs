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
using System.ComponentModel.Design;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.Modal_Properties.PESB;

namespace VigilanceClearance.Controllers
{
    public class PESBController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IPESB _pesb;
        public PESBController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration, IPESB pesb)
        {
            this._clientFactory = clientFactory;
            this._httpClient = httpClient;
            this._configuration = configuration;
            this._pesb = pesb;
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
            ViewBag.title = "PESB : Add New Reference";
            var ReferenceCode = "APPOINTMENT";
            string id = "0";
            try
            {
                ViewBag.title = "PESB : Add New Reference";

                var model = new PESBViewModel
                {
                    ReferenceDescList = await _pesb.GetReferenceDropDownAsync(),
                    PostDescriptionList = await _pesb.GetPostDescriptionsDropDownAsync(ReferenceCode),
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

        [HttpGet]
        public async Task<IActionResult> PESB_Appointment(int id)
        {
            try
            {
                ViewBag.title = "PESB Appointment";

                string username = HttpContext.Session.GetString("Username");
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account");
                }

                var model = new VcReferenceReceivedForGetById_Model
                {
                    data_List = await _pesb.Get_VC_ReferenceReceivedFor_List_GetByIdAsync(id, username)
                };
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }



        [HttpGet]
        public async Task<IActionResult> PESB_Add_New_Reference()
        {
            ViewBag.title = "PESB : Add New Reference";
            var referenceReceivedFor = "APPOINTMENT";
            string id = "0";
            try
            {
                var model = new AddNewReference_VM
                {
                    referenceReceivedFor_List = await _pesb.GetReferenceDropDownAsync(),
                    selectionForThePostCategory_List = await _pesb.GetPostDescriptionsDropDownAsync(referenceReceivedFor),
                    selectionForThePostSubCategory_List = new List<SelectListItem>(),
                    orgName_List = await _pesb.GetOrganizationDropDownAsync(id),
                    minDesc_List = new List<SelectListItem>(),
                    referenceReceivedFor = "APPOINTMENT"
                };

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetSubPostsByPostCode(string postCode)
        {
            try
            {
                var subPosts = await _pesb.GetSubPostsByPostCodeInternal(postCode);
                return Json(subPosts);
            }
            catch
            {
                return Json(new { Error = "Failed to load sub-posts." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetMinistryByOrgCode(string id)
        {
            try
            {
                var ministry = await _pesb.GetMinistryByPostCodeInternal(id);
                return Json(ministry);
            }
            catch
            {
                return Json(new { Error = "Failed to load sub-posts." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> PESB_Add_New_Reference(AddNewReference_VM model)
        {
            PESB_Add_New_Reference_Model objmodel = new PESB_Add_New_Reference_Model();
            ViewBag.title = "PESB : Add New Reference";
            string id = "0";
            try
            {
                // Remove list properties from ModelState to exclude them from validation
                ModelState.Remove("ReferenceReceivedFor_List");
                ModelState.Remove("selectionForThePostCategory_List");
                ModelState.Remove("selectionForThePostSubCategory_List");
                ModelState.Remove("orgName_List");
                ModelState.Remove("minDesc_List");

                if (!ModelState.IsValid)
                {
                    // Re-populate dropdown lists for view rendering
                    model.referenceReceivedFor_List = await _pesb.GetReferenceDropDownAsync();
                    model.selectionForThePostCategory_List = await _pesb.GetPostDescriptionsDropDownAsync(model.referenceReceivedFor ?? "APPOINTMENT");
                    model.selectionForThePostSubCategory_List = new List<SelectListItem>();
                    model.orgName_List = await _pesb.GetOrganizationDropDownAsync(id);
                    model.minDesc_List = new List<SelectListItem>();
                    model.referenceReceivedFor = "APPOINTMENT";
                    return View(model);
                }
                else
                {
                    //if (model.selectionForThePostSubCategory == "Other" && string.IsNullOrWhiteSpace(model.OtherSubPost))
                    //{
                    //    ModelState.AddModelError("OtherSubPost", "Please enter sub post.");
                    //}
                    objmodel.referenceReceivedFor = model.referenceReceivedFor;
                    objmodel.referenceReceivedFrom = "PESB";
                    objmodel.referenceReceivedFromCode = "PESB";

                    objmodel.selectionForThePostCategory = model.selectionForThePostCategory;
                    objmodel.selectionForThePostSubCategory = model.selectionForThePostSubCategory;

                    objmodel.orgCode = model.orgCode;
                    objmodel.orgName = null;

                    objmodel.minCode = model.minCode;
                    objmodel.minDesc = null;

                    objmodel.referenceNoFileNo = model.referenceNoFileNo;
                    objmodel.referenceOrSubmissionToCvcDate = model.referenceOrSubmissionToCvcDate;
                    objmodel.cvcReferenceIdFileNo = model.cvcReferenceIdFileNo;

                    objmodel.createdBy = HttpContext.Session.GetString("Username");
                    objmodel.createdOn = DateTime.Now;
                    objmodel.createdBySessionId = HttpContext.Session.Id;
                    objmodel.createdByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                    objmodel.updateBy = null;
                    objmodel.updatedOn = null;
                    objmodel.updatedBySessionId = null;
                    objmodel.updatedByIp = null;

                    objmodel.pendingWith = "CVC";
                    objmodel.uid = null;
                    objmodel.referenceId = null;

                    int num = await _pesb.InsertAddNewReferenceAsync(objmodel);
                    if (num == 0)
                    {
                        ModelState.AddModelError(string.Empty, "Record not saved.");
                        model.referenceReceivedFor_List = await _pesb.GetReferenceDropDownAsync();
                        model.selectionForThePostCategory_List = await _pesb.GetPostDescriptionsDropDownAsync(model.referenceReceivedFor ?? "APPOINTMENT");
                        model.selectionForThePostSubCategory_List = new List<SelectListItem>();
                        model.orgName_List = await _pesb.GetOrganizationDropDownAsync(id);
                        model.minDesc_List = new List<SelectListItem>();
                        return View("PESB_Add_New_Reference", model);
                    }
                    else
                    {
                        TempData["FormAction"] = "submit"; // or "forward"
                        TempData["SuccessMessage"] = "Reference added successfully!";
                        return RedirectToAction("PESB_Add_New_Reference");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                model.referenceReceivedFor_List = await _pesb.GetReferenceDropDownAsync();
                model.selectionForThePostCategory_List = await _pesb.GetPostDescriptionsDropDownAsync(model.referenceReceivedFor ?? "APPOINTMENT");
                model.selectionForThePostSubCategory_List = await _pesb.GetSubPostsByPostCodeInternal(model.selectionForThePostCategory ?? "");
                model.orgName_List = await _pesb.GetOrganizationDropDownAsync("0");
                model.minDesc_List = string.IsNullOrWhiteSpace(model.minCode)
                    ? new List<SelectListItem>()
                    : await _pesb.GetMinistryByPostCodeInternal(model.minCode);
                return View("PESB_Add_New_Reference", model);
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
                    //model.PostDescriptionList = await GetPostDescriptionsDropDownAsync("APPOINTMENT");
                    //model.SubPostDescriptionList = await GetSubPostsByPostCodeInternal("");
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
                //model.PostDescriptionList = await GetPostDescriptionsDropDownAsync("APPOINTMENT");
                //model.SubPostDescriptionList = await GetSubPostsByPostCodeInternal("");
                return View(model);
            }
        }




        [HttpPost]
        public async Task<IActionResult> Reports(PESB_Add_New_Reference_Model model)
        {
            ViewBag.title = " ";
            try
            {
                
                return RedirectToAction();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                //model.PostDescriptionList = await GetPostDescriptionsDropDownAsync("APPOINTMENT");
                //model.SubPostDescriptionList = await GetSubPostsByPostCodeInternal("");
                return View(model);
            }
        }


    }
}