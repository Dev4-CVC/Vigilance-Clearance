using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Reflection.Metadata;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Controllers
{
    [Authorize(Roles = "User")]
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

        [HttpGet]
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
        public async Task<IActionResult> PESB_Appointment()
        {
            int id = 0;
            try
            {
                ViewBag.title = "PESB Appointment";

                string username = HttpContext.Session.GetString("Username");
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account");
                }

                var model = new PESBViewModel
                {
                    new_reference_List = await _pesb.Get_Vc_Reference_Received_For_List_GetById_and_Username_Async(id, username)
                };
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }




        //==================================================================
        //new operation:  dated on 9 july 2025
        //New Reference 
        [HttpGet]
        public async Task<IActionResult> new_reference()
        {
            ViewBag.title = "Add New Reference";
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }
            var referenceReceivedFor = "APPOINTMENT";
            string id = "0";
            try
            {
                var model = new PESBViewModel
                {
                    new_reference = new new_reference_model
                    {
                        referenceReceivedFor = referenceReceivedFor
                    },
                    reference_received_for_ddl_List = await _pesb.GetReferenceDropDownAsync(),
                    post_ddl_List = await _pesb.GetPostDescriptionsDropDownAsync(referenceReceivedFor),
                    sub_post_ddl_List = new List<SelectListItem>(),
                    organization_ddl_List = await _pesb.GetOrganizationDropDownAsync(id),
                    ministry_ddl_List = new List<SelectListItem>(),
                };
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> new_reference(PESBViewModel objmodel)
        {
            ViewBag.title = "Add New Reference";
            string id = "0";
            try
            {
                
                objmodel.new_reference.referenceReceivedFor = objmodel.new_reference.referenceReceivedFor;
                objmodel.new_reference.referenceReceivedFrom = "PESB";
                objmodel.new_reference.referenceReceivedFromCode = "PESB";
                objmodel.new_reference.selectionForThePostCategory = objmodel.new_reference.selectionForThePostCategory;
                objmodel.new_reference.selectionForThePostSubCategory= objmodel.new_reference.selectionForThePostSubCategory;

                objmodel.new_reference.orgCode = objmodel.new_reference.orgCode;
                objmodel.new_reference.minCode = objmodel.new_reference.minCode;
                objmodel.new_reference.orgName = null;
                objmodel.new_reference.minDesc = null;

                objmodel.new_reference.referenceNoFileNo = objmodel.new_reference.referenceNoFileNo;
                objmodel.new_reference.referenceOrSubmissionToCvcDate= objmodel.new_reference.referenceOrSubmissionToCvcDate;
                objmodel.new_reference.cvcReferenceIdFileNo= objmodel.new_reference.cvcReferenceIdFileNo;

                objmodel.new_reference.createdBy = HttpContext.Session.GetString("Username");
                objmodel.new_reference.createdByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                objmodel.new_reference.createdBySessionId = HttpContext.Session.Id;
                objmodel.new_reference.createdOn = DateTime.Now;

                objmodel.new_reference.updateBy = null;
                objmodel.new_reference.updatedByIp = null;
                objmodel.new_reference.updatedBySessionId = null;
                objmodel.new_reference.updatedOn = null;

                objmodel.new_reference.pendingWith = "PESB";
                objmodel.new_reference.referenceId = "ABC123";

                int isInserted = await _pesb.Insert_Add_New_Reference_Async(objmodel.new_reference);
                if (isInserted > 0)
                {
                    // return Json(new { success = true });
                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("PESB_Appointment")
                    });
                }
                else
                {
                    return Json(new { success = false, error_new_reference_message = "This record is not saved." });
                }

            }
            catch (Exception ex)
            {
                return View("new_reference");
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

        [HttpGet]
        public async Task<IActionResult> new_reference_detailss(int id)
        {
            ViewBag.title = "Reference Details";
            try
            {
                HttpContext.Session.SetInt32("RefId", id);

                int? referenceid = HttpContext.Session.GetInt32("RefId");

                if (referenceid.HasValue)
                {
                    ViewBag.ReferenceId = referenceid.Value;
                }

                var result = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(id);
                var data = result.FirstOrDefault();
                if (data == null)
                {
                    ViewBag.Error = "No data found.";
                    return View();
                }

                var model = new PESBViewModel
                {
                    new_reference = data,
                    officer_details_List = await _pesb.Get_Officer_List_GetById_Async(id)
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
        public async Task<IActionResult> Reference_Details(int id)
        {
            ViewBag.title = "Reference Details";
            try
            {
                HttpContext.Session.SetInt32("RefId", id);

                var result = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(id);
                var data = result.FirstOrDefault();
                if (data == null)
                {
                    ViewBag.Error = "No data found.";
                    return View();
                }

                var model = new PESBViewModel
                {
                    new_reference = data,
                    officer_details_List = await _pesb.Get_Officer_List_GetById_Async(id)
                };

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


        //Officer Details and Officer Posting Details
        [HttpGet]
        public async Task<IActionResult> Officer_Reports()
        {
            ViewBag.title = "Officer Details Reports";
            string id = "0";


            int? referenceid = HttpContext.Session.GetInt32("RefId");

            if (!referenceid.HasValue)
            {
                ViewBag.Error = "Reference ID not found in session.";
                return View(); // Or redirect back to a safe page
            }

            ViewBag.ReferenceId = referenceid.Value;
            try
            {
                var model = new PESBViewModel
                {
                    service_ddl_List = await _pesb.Get_Service_DropDownAsync(),
                    batch_ddl_List = await _pesb.Get_Batch_DropDownAsync(),
                    cadre_ddl_List = await _pesb.Get_Cadre_DropDownAsync(),
                    organization_ddl_List = await _pesb.GetOrganizationDropDownAsync(id),
                    ministry_ddl_List = new List<SelectListItem>(),
                    //officer_details_List = await _pesb.Get_Officer_List_GetById_Async(referenceid.Value),
                    //officer_posting_details_List = await _pesb.Get_Officer_Posting_List_GetById_Async(referenceid.Value),
                };
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Officer_Reports([FromBody] PESBViewModel objmodel)
        {
            try
            {
                int? referenceid = HttpContext.Session.GetInt32("RefId");

                int? id = referenceid;

                if (!referenceid.HasValue)
                {
                    ViewBag.Error = "Reference ID not found in session.";
                    return View(); // Or redirect back to a safe page
                }

                var _service = objmodel.officer_details.officer_Service;
                var Otherservice = string.Empty;
                var _Cadre = string.Empty;

                if (_service == "OTHERS")
                    Otherservice = objmodel.officer_details.officer_other_Service;
                else
                    objmodel.officer_details.officer_other_Service = null;

                if (_service == "IPS" || _service == "IAS" || _service == "IFoS")
                    _Cadre = objmodel.officer_details.officer_Cadre;
                else
                    objmodel.officer_details.officer_Cadre = null;

                // Assign values from model
                objmodel.officer_details.masterReferenceID = (long)referenceid;

                objmodel.officer_details.officer_DateOfBirth = (DateTime)objmodel.officer_details.officer_DateOfBirth;
                objmodel.officer_details.officer_RetirementDate = (DateTime)objmodel.officer_details.officer_RetirementDate;
                objmodel.officer_details.officer_ServiceEntryDate = (DateTime)objmodel.officer_details.officer_ServiceEntryDate;

                objmodel.officer_details.officer_Service = _service;
                objmodel.officer_details.officer_other_Service = Otherservice;
                objmodel.officer_details.officer_Cadre = _Cadre;
                objmodel.officer_details.officer_Batch_Year = objmodel.officer_details.officer_Batch_Year;

                objmodel.officer_details.createdBy = HttpContext.Session.GetString("Username");
                objmodel.officer_details.createdBy_SessionId = HttpContext.Session.Id;
                objmodel.officer_details.createdBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                int num = await _pesb.Insert_Officer_Details_Async(objmodel.officer_details);
                if (num == 0)
                {
                    return Json(new { success = false, message = "Record not saved." });
                }


                // ✅ Return JSON with redirect URL
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Reference_Details", new { id = id })
                });
                // return RedirectToAction("Reference_Details", new { id = id });
                //return Json(new { success = true, redirectUrl = Url.Action("Reference_Details", new { id = id }) });
                // ✅ Return reference ID for client-side redirect
                // return Json(new { success = true, masterReferenceID = referenceid });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> officer_posting_details()
        {
            ViewBag.title = "Officer Posting Details ";
            string id = "0";

            int? officerid = HttpContext.Session.GetInt32("officerId");
            int? referenceid = HttpContext.Session.GetInt32("RefId");

            if (!officerid.HasValue)
            {
                ViewBag.Error = "Reference ID not found in session.";
                return View(); // Or redirect back to a safe page
            }

            ViewBag.ReferenceId = officerid.Value;
            try
            {
                var model = new PESBViewModel
                {
                    OfficerId = officerid ?? 0,
                    ReferenceId = referenceid ?? 0,
                    officer_posting_details = new officer_posting_details_model(), // 👈 this line is critical!
                    organization_ddl_List = await _pesb.GetOrganizationDropDownAsync(id),
                    ministry_ddl_List = new List<SelectListItem>(),
                    //officer_details_List = await _pesb.Get_Officer_List_GetById_Async(referenceid.Value),
                    //officer_posting_details_List = await _pesb.Get_Officer_Posting_List_GetById_Async(referenceid.Value),
                };
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> officer_posting_details([FromBody] PESBViewModel objmodel)
        {
            string id = "0";

            int? officerid = HttpContext.Session.GetInt32("officerId");
            int? referenceid = HttpContext.Session.GetInt32("RefId");
            int? officerpostingid = HttpContext.Session.GetInt32("officerPostingId");

            if (!officerid.HasValue || !referenceid.HasValue)
            {
                ViewBag.Error = "Required session values not found.";
                return View(objmodel); // Return with error
            }

            try
            {
                objmodel.officer_posting_details.vcOfficerId = officerid.Value;
                objmodel.officer_posting_details.MasterReferenceId = referenceid.Value;
                objmodel.officer_posting_details.orgCode = objmodel.officer_posting_details.orgCode;
                objmodel.officer_posting_details.orgMinistry = objmodel.officer_posting_details.orgMinistry;
                objmodel.officer_posting_details.designation = objmodel.officer_posting_details.designation;
                objmodel.officer_posting_details.placeOfPosting = objmodel.officer_posting_details.placeOfPosting;
                objmodel.officer_posting_details.fromDate = (DateTime)objmodel.officer_posting_details.fromDate;
                objmodel.officer_posting_details.toDate = (DateTime)objmodel.officer_posting_details.toDate;

                objmodel.officer_posting_details.actionBy = HttpContext.Session.GetString("Username");
                objmodel.officer_posting_details.actionBy_SessionId = HttpContext.Session.Id;
                objmodel.officer_posting_details.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                int result = await _pesb.Insert_Officer_Posting_Details_Async(objmodel.officer_posting_details);
                if (result == 0)
                {
                    return Json(new { success = false, message = "Record not saved." });
                }
                
                var redirectUrl = Url.Action("ViewOfficerDetails", new { id = officerid, masterrefid = referenceid });
                return Json(new { success = true, redirectUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Officer_list(int id)
        {
            ViewBag.title = "Officer Details";
            try
            {
                string username = HttpContext.Session.GetString("Username");
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account");
                }

                HttpContext.Session.SetInt32("RefId", id);

                int? referenceid = HttpContext.Session.GetInt32("RefId");

                if (referenceid.HasValue)
                {
                    ViewBag.ReferenceId = referenceid.Value;
                }

                var model = new PESBViewModel
                {
                    officer_details_List = await _pesb.Get_Officer_List_GetById_Async(id)
                };

                if (model.officer_details_List == null || !model.officer_details_List.Any())
                {
                    ViewBag.Error = "No data found.";
                    return View(model);
                }

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Officer_Details(int masterReferenceID)
        {
            ViewBag.title = "Reference Details";
            try
            {
                int referenceid = (int)HttpContext.Session.GetInt32("RefId");

                var masterreferenceresult = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(masterReferenceID);
                var modelreference = masterreferenceresult.FirstOrDefault();
                if (masterreferenceresult == null)
                {
                    ViewBag.Error = "No data found.";
                    return View();
                }
                var model = new PESBViewModel
                {
                    new_reference = modelreference,
                    officer_details_List = await _pesb.Get_Officer_List_GetById_Async(referenceid)
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
        public async Task<IActionResult> ForwardedToCVC(int id)
        {
            try
            {


                PESB_Update_Referencefrom_model objmodel = new PESB_Update_Referencefrom_model();
                objmodel.MainReferenceID = id;
                objmodel.UpdatedBy = HttpContext.Session.GetString("Username");
                objmodel.UpdatedBy_SessionId = HttpContext.Session.Id;
                objmodel.UpdatedBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                objmodel.PendingWith = "CVC_COORD2_DH";
                int result = await _pesb.Update_Reference_From_PESB_Async(objmodel);

                if (result == 1)
                {
                    return RedirectToAction("PESB_Appointment");
                }
                else
                {
                    return Json(new { success = false, message = "Data not updated." });

                }

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }


        [HttpGet("ViewOfficerDetails/{id}/{masterrefid}")]
        public async Task<IActionResult> ViewOfficerDetails(int id, int masterrefid)
            {
            try
            {
                HttpContext.Session.SetInt32("officerId", id);
                HttpContext.Session.SetInt32("masterRefId", masterrefid);


                var Reference_Received_Details = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(masterrefid);
                var Officer_Details = await _pesb.Get_Officer_Details_Async(id);

                var data1 = Reference_Received_Details.FirstOrDefault();
                var data2 = Officer_Details.FirstOrDefault();
                if (data1 == null || data2 == null)
                {
                    ViewBag.Error = "No data found.";
                    return View();
                }
                var model = new PESBViewModel
                {
                    new_reference = data1,
                    officer_details = data2,
                    officer_posting_details_List = await _pesb.Get_Officer_Posting_List_GetById_Async(id)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}
