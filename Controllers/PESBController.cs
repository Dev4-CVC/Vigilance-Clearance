using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.PESB;

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


        //Insert Operation --> Details pf the officer
        //[HttpGet("PESB/PESBReports/{referenceid?}")]
        public async Task<IActionResult> PESBReports()
        {
            ViewBag.title = "PESB : Reports";
            string id = "0";
            //if (referenceid.HasValue)
            //{
            //    ViewBag.ReferenceId = referenceid.Value;
            //}
            try
            {
                var model = new OfficerDetails_VM
                {
                    Service_List = await _pesb.Get_Service_DropDownAsync(),
                    Batch_List = await _pesb.Get_Batch_DropDownAsync(),
                    Cadre_List = await _pesb.Get_Cadre_DropDownAsync(),
                    orgName_List = await _pesb.GetOrganizationDropDownAsync(id),
                    minDesc_List = new List<SelectListItem>()
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
        public async Task<IActionResult> PESBReports(OfficerDetails_VM model)
        {
            OfficerDetails_Model objmodel = new OfficerDetails_Model();
            ViewBag.title = "PESB Report Form";
            try
            {
                ModelState.Remove("Service_List");
                ModelState.Remove("Batch_List");
                ModelState.Remove("Cadre_List");
                ModelState.Remove("orgName_List");
                ModelState.Remove("minDesc_List");
                ModelState.Remove("orgCode");
                ModelState.Remove("minCode");
                ModelState.Remove("OfficerId");
                ModelState.Remove("Designation");
                ModelState.Remove("PlaceOfPosting");
                ModelState.Remove("FromDate");
                ModelState.Remove("ToDate");

                //var _service = model.officer_Service;
                var _service = model.officer_Service;
                var Otherservice = string.Empty;
                var _Cadre = string.Empty;

                if (_service == "OTHERS")
                {
                    Otherservice = model.OtherService;
                }
                else
                {
                    ModelState.Remove("OtherService");
                }

                if (_service == "IPS" || _service == "IAS" || _service == "IFoS")
                {
                    _Cadre = model.officer_Cadre;
                }
                else
                {
                    ModelState.Remove("officer_Cadre");
                }

                // Validate model
                if (!ModelState.IsValid)
                {
                    model.Service_List = await _pesb.Get_Service_DropDownAsync();
                    model.Batch_List = await _pesb.Get_Batch_DropDownAsync();
                    model.Cadre_List = await _pesb.Get_Cadre_DropDownAsync();
                    return View(model);
                }

                // Assign values from model
                objmodel.masterReferenceID = model.referenceid;

                objmodel.officer_Name = model.officer_Name;
                objmodel.officer_FatherName = model.officer_FatherName;
                objmodel.officer_DateOfBirth = (DateTime)model.officer_DateOfBirth;
                objmodel.officer_RetirementDate = (DateTime)model.officer_RetirementDate;
                objmodel.officer_ServiceEntryDate = (DateTime)model.officer_ServiceEntryDate;
                objmodel.officer_Service = _service;
                objmodel.officer_other_Service = Otherservice;
                objmodel.officer_Cadre = _Cadre;
                objmodel.officer_Batch_Year = model.officer_Batch_Year;

                objmodel.createdBy = HttpContext.Session.GetString("Username");
                objmodel.createdBy_SessionId = HttpContext.Session.Id;
                objmodel.createdBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                // Call API
                int num = await _pesb.InsertOfficerDetailsAsync(objmodel);
                if (num == 0)
                {
                    ModelState.AddModelError(string.Empty, "Record not saved.");
                    model.Service_List = await _pesb.Get_Service_DropDownAsync();
                    model.Batch_List = await _pesb.Get_Batch_DropDownAsync();
                    model.Cadre_List = await _pesb.Get_Cadre_DropDownAsync();
                    return View("PESBReports", model);
                }

                TempData["SuccessMessage_Of_officer_details"] = "Officer details added successfully!";
                return RedirectToAction("PESBReports");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> OfficerPostingDetails(OfficerDetails_VM model)
        {
            string id = "0";
            OfficerPostingDetails_Model objmodel = new OfficerPostingDetails_Model();

            model.OfficerId = 1;
            ModelState.Remove("officer_Name");
            ModelState.Remove("officer_FatherName");
            ModelState.Remove("officer_DateOfBirth");
            ModelState.Remove("officer_ServiceEntryDate");
            ModelState.Remove("officer_RetirementDate");
            ModelState.Remove("officer_Service");
            ModelState.Remove("officer_Batch_Year");
            ModelState.Remove("officer_Cadre");
            ModelState.Remove("OtherService");
            ModelState.Remove("Service_List");
            ModelState.Remove("Batch_List");
            ModelState.Remove("Cadre_List");
            ModelState.Remove("orgName_List");
            ModelState.Remove("minDesc_List");

            if (!ModelState.IsValid)
            {
                model.orgName_List = await _pesb.GetOrganizationDropDownAsync(id);
                model.minDesc_List = new List<SelectListItem>();
                return View(model);
            }

            try
            {
                objmodel.vcOfficerId = model.OfficerId;
                objmodel.orgCode = model.orgCode;
                objmodel.orgMinistry = model.minCode;
                objmodel.designation = model.Designation;
                objmodel.placeOfPosting = model.PlaceOfPosting;
                objmodel.fromDate = (DateTime)model.FromDate;
                objmodel.toDate = (DateTime)model.ToDate;

                objmodel.actionBy = HttpContext.Session.GetString("Username");
                objmodel.actionBy_SessionId = HttpContext.Session.Id;
                objmodel.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";


                // Call API
                int num = await _pesb.InsertOfficerPostingDetailsAsync(objmodel);
                if (num == 0)
                {
                    ModelState.AddModelError(string.Empty, "Record not saved.");
                    model.Service_List = await _pesb.Get_Service_DropDownAsync();
                    model.Batch_List = await _pesb.Get_Batch_DropDownAsync();
                    model.Cadre_List = await _pesb.Get_Cadre_DropDownAsync();
                    return View("PESBReports", model);
                }

                TempData["SuccessMessage_Of_officer_positing_details"] = "Officer posting details added successfully!";
                return RedirectToAction("PESBReports");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OfficerDetails()
        {
            int id = 0;
            try
            {
                ViewBag.title = "Officer Details";

                string username = HttpContext.Session.GetString("Username");
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("Login", "Account");
                }

                //var model = new OfficerDetails_Model
                //{
                //    data_List = await _pesb.Get_VC_ReferenceReceivedFor_List_GetByIdAsync(id, username)
                //};
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
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

            //int id = 0;
            //try
            //{
            //    ViewBag.title = "PESB Appointment";

            //    string username = HttpContext.Session.GetString("Username");
            //    if (string.IsNullOrEmpty(username))
            //    {
            //        return RedirectToAction("Login", "Account");
            //    }

            //    var model = new VcReferenceReceivedFor_VM
            //    {
            //        data_List = await _pesb.Get_VC_ReferenceReceivedFor_List_GetByIdAsync(id, username)
            //    };
            //    return View(model);
            //}
            //catch (Exception)
            //{
            //    ViewBag.Error = "Something went wrong while loading the page.";
            //    return View();
            //}
        }



        //Add New Reference Insert Operation
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

        [HttpPost]
        public async Task<IActionResult> PESB_Add_New_Reference(AddNewReference_VM model)
        {
            PESB_Add_New_Reference_Model objmodel = new PESB_Add_New_Reference_Model();
            ViewBag.title = "PESB : Add New Reference";
            string id = "0";
            try
            {
                ModelState.Remove("ReferenceReceivedFor_List");
                ModelState.Remove("selectionForThePostCategory_List");
                ModelState.Remove("selectionForThePostSubCategory_List");
                ModelState.Remove("orgName_List");
                ModelState.Remove("minDesc_List");

                if (!ModelState.IsValid)
                {
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
                        return RedirectToAction("PESB_Appointment", "PESB");
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

        [HttpGet]
        public async Task<IActionResult> New_Reference_Details(int id)
        {
            ViewBag.title = "PESB: Reference Details";
            try
            {
                var result = await _pesb.Get_VC_ReferenceReceivedFor_Details_Async(id);
                var model = result.FirstOrDefault();
                if (model == null)
                {
                    ViewBag.Error = "No data found.";
                    return View();
                }
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
                objmodel.new_reference.referenceReceivedFrom = "PESB";
                objmodel.new_reference.referenceReceivedFromCode = "PESB";

                objmodel.new_reference.orgName = null;
                objmodel.new_reference.minDesc = null;

                objmodel.new_reference.createdBy = HttpContext.Session.GetString("Username");
                objmodel.new_reference.createdByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                objmodel.new_reference.createdBySessionId = HttpContext.Session.Id;
                objmodel.new_reference.createdOn = DateTime.Now;

                objmodel.new_reference.updateBy = null;
                objmodel.new_reference.updatedByIp = null;
                objmodel.new_reference.updatedBySessionId = null;
                objmodel.new_reference.updatedOn = null;

                objmodel.new_reference.pendingWith = "CVC";
                objmodel.new_reference.uid = null;
                objmodel.new_reference.referenceId = null;

                int isInserted = await _pesb.Insert_Add_New_Reference_Async(objmodel.new_reference);
                if (isInserted > 0)
                {
                    return Json(new { success = true });
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
                var result = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(id);
                var model = result.FirstOrDefault();
                if (model == null)
                {
                    ViewBag.Error = "No data found.";
                    return View();
                }
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


        //Officer Details and Officer Posting Details
        public async Task<IActionResult> Officer_Reports()
        {
            ViewBag.title = "Officer Reports";
            string id = "0";
            //if (referenceid.HasValue)
            //{
            //    ViewBag.ReferenceId = referenceid.Value;
            //}
            try
            {
                var model = new PESBViewModel
                {
                    service_ddl_List = await _pesb.Get_Service_DropDownAsync(),
                    batch_ddl_List = await _pesb.Get_Batch_DropDownAsync(),
                    cadre_ddl_List = await _pesb.Get_Cadre_DropDownAsync(),
                    organization_ddl_List = await _pesb.GetOrganizationDropDownAsync(id),
                    ministry_ddl_List = new List<SelectListItem>()
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
                objmodel.officer_details.masterReferenceID = 1;

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

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> officer_posting_details([FromBody] PESBViewModel objmodel)
        {
            string id = "0";

            objmodel.officer_posting_details.vcOfficerId = 1;
            objmodel.officer_posting_details.orgCode = objmodel.officer_posting_details.orgCode;
            objmodel.officer_posting_details.orgMinistry = objmodel.officer_posting_details.orgMinistry;
            objmodel.officer_posting_details.designation = objmodel.officer_posting_details.designation;
            objmodel.officer_posting_details.placeOfPosting = objmodel.officer_posting_details.placeOfPosting;
            objmodel.officer_posting_details.fromDate = (DateTime)objmodel.officer_posting_details.fromDate;
            objmodel.officer_posting_details.toDate = (DateTime)objmodel.officer_posting_details.toDate;

            objmodel.officer_posting_details.actionBy = HttpContext.Session.GetString("Username");
            objmodel.officer_posting_details.actionBy_SessionId = HttpContext.Session.Id;
            objmodel.officer_posting_details.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            try
            {
                int result = await _pesb.Insert_Officer_Posting_Details_Async(objmodel.officer_posting_details);

                if (result == 0)
                {
                    return Json(new { success = false, message = "Record not saved." });
                }

                return Json(new { success = true });
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}