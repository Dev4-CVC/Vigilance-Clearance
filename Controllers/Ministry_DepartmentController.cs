using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models;
using VigilanceClearance.Models.DTOs;
using VigilanceClearance.Models.Ministry_ApproverModels;
using VigilanceClearance.Models.Modal_Properties;
using VigilanceClearance.Models.Modal_Properties.Account;
using VigilanceClearance.Models.New_Reference_to_CVCModels;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.Ministry;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Controllers
{
    [Authorize(Roles = "MINISTRY_DH,MINISTRY_APPROVER,MINISTRY_CVO")]
    public class Ministry_DepartmentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string BaseUrl;
        private readonly IMinistry _ministry;
        private readonly IPESB _pesb;

        public Ministry_DepartmentController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration, IMinistry ministry, IPESB pesb)
        {
            this._clientFactory = clientFactory;
            _httpClient = httpClient;
            _configuration = configuration;
            BaseUrl = _configuration["BaseUrl"]!;
            this._ministry = ministry;
            _pesb = pesb;
        }

        public IActionResult Index()
        {
            try
            {
                var userDetailsJson = HttpContext.Session.GetString("UserDetails");
                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _MinMastercode = userDetails.MinMasterCode;

                   TempData["Role"] =  HttpContext.Session.GetString("UserRole");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet]
        public async Task<IActionResult> ViewReferenceReceivedCVC()
        {
            try
            {
                var userDetailsJson = HttpContext.Session.GetString("UserDetails");
                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _MinMastercode = userDetails.MinMasterCode;
                    string _Mincode = userDetails.MinCode;
                    //List<ReferenceReceivedFromCVCModel> apiResponse = await _ministry.GetReferenceReceivedFromCVClist(_MinMastercode);
                    List<References_from_coord2_To_MinistryModel> apiResponse = await _ministry.References_from_coord2_To_Ministry(_Mincode);
                    if (apiResponse != null)
                    {
                        return View(apiResponse);
                    }
                    return View(new List<References_from_coord2_To_MinistryModel>());
                }
                else
                {
                    return RedirectToAction("LogoutRedirect", "Account");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewOfficerListForGivenProposal(string id)
        {
            try
            {
                List<OfficerListModel> ApiResponse = await _ministry.GetOfficerListAsync(id);

                if (ApiResponse != null)
                {
                    return View(ApiResponse); //  Send list directly to view
                }

                return View(new List<OfficerListModel>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateReferenceReceivedCVC(string id, string refId , string flag)
        {
            try
            {
                HttpContext.Session.SetString("vcofficerId", id);
                HttpContext.Session.SetString("MasterRefID", refId);
                HttpContext.Session.SetString("flag", flag);

                TempData["RefId"] = id;  // Iska use back button ke liye ho raha hai / agar first box se aana hai to refid jayegi or second se aaya hai to id (Matlab officerid) jayegi

                List<OfficerListModel> Response = await _ministry.GetOfficerDetailsByOfficerIdAsync(id);

                OfficerListModel firstOfficer = Response.FirstOrDefault();
                //7
                List<OfficerPostingDetailsViewModellist> Postinglist = await _ministry.GetOfficerPostingList(id);
                //8
                List<InsertIntegrityAgreedOrDoubtfulModel> IntegrityAgreedlist = await _ministry.GetInsertIntegrityAgreedOrDoubtfulList(id);

                //9           
                List<AllegationOfMisconductExaminedModel> AllegationOfMisconductlist = await _ministry.GetAllegationOfMisconductExaminedList(id);

                //10
                List<PunishmentAwardedModel> PunishmentAwardedlist = await _ministry.GetPunishmentAwardedList(id);

                //11 
                List<DisciplinaryCriminalProceedingsModel> DisciplinaryCriminalProceedingsModellist = await _ministry.GetDisciplinaryCriminalProceedingsModelList(id);

                //12
                List<ActionContemplatedAgainstTheOfficerModel> ActionContemplatedAgainstTheOfficerlist = await _ministry.GetActionContemplatedAgainstTheOfficerlList(id);

                //13
                List<ComplaintWithVigilanceAnglePendingModel> ComplaintWithVigilanceAnglePendingModellist = await _ministry.GetComplaintWithVigilanceAnglePendingList(id);


                var result = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(int.Parse(refId));


                string orgcode = string.Empty;

                var model = new OfficerDetailMainModel
                {
                    officerPostingDetail7 = new OfficerPostingDetails
                    {
                        OrganizationList = await _ministry.GetOrganizationDropDownAsync("0"),
                        Organization = null // or default
                    },

                    officerPersonalDetailModel = new OfficerPersonalDetailModel
                    {
                        Officer_Name = firstOfficer.Officer_Name,
                        Officer_FatherName = firstOfficer.Officer_FatherName,
                        Officer_DateOfBirth = firstOfficer.Officer_DateOfBirth.ToString("dd-MM-yyyy"),
                        Officer_RetirementDate = firstOfficer.Officer_RetirementDate.ToString("dd-MM-yyyy"),
                        Officer_ServiceEntryDate = firstOfficer.Officer_ServiceEntryDate.ToString("dd-MM-yyyy"),
                        Officer_Service = firstOfficer.Officer_Service,
                        Officer_Batch_Year = firstOfficer.Officer_Batch_Year.ToString(),
                        Cadre = firstOfficer.Officer_Cadre
                    },

                    officerPostingDetailsList = Postinglist,
                    insertIntegrityAgreedOrDoubtfulModellist = IntegrityAgreedlist,
                    AllegationOfMisconductExaminedModellist = AllegationOfMisconductlist,
                    PunishmentAwardedModellist = PunishmentAwardedlist,
                    DisciplinaryCriminalProceedingsModellist = DisciplinaryCriminalProceedingsModellist,
                    ActionContemplatedAgainstTheOfficerModellist = ActionContemplatedAgainstTheOfficerlist,
                    ComplaintWithVigilanceAnglePendingModellist = ComplaintWithVigilanceAnglePendingModellist,

                    new_Reference_Model = result.FirstOrDefault(),
                };

                return View(model);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }


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


        [HttpPost]
        public async Task<IActionResult> AddOfficerPostingDetails(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {
                InsertOfficerDetailsModel insertOfficerDetailsModel = new InsertOfficerDetailsModel();
                string Id = HttpContext.Session.GetString("vcofficerId");                
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");
                try
                {
                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerPostingDetail7, nameof(model.officerPostingDetail7)))
                    {
                        // Submodel is valid!
                        insertOfficerDetailsModel.masterReferenceId = int.Parse(MasterRefId);
                        insertOfficerDetailsModel.OrgCode = model.officerPostingDetail7.Organization;
                        insertOfficerDetailsModel.OrgMinistry = model.officerPostingDetail7.Ministry;
                        insertOfficerDetailsModel.Designation = model.officerPostingDetail7.Designation;
                        insertOfficerDetailsModel.PlaceOfPosting = model.officerPostingDetail7.PlaceOfPosting;
                        insertOfficerDetailsModel.VcOfficerId = int.Parse(Id);
                        string fromDate = DateTime.Parse(model.officerPostingDetail7.TenureFrom.ToString()).ToString("yyyy-MM-dd");
                        string toDate = DateTime.Parse(model.officerPostingDetail7.TenureTo.ToString()).ToString("yyyy-MM-dd");

                        insertOfficerDetailsModel.FromDate = DateTime.Parse(fromDate);
                        insertOfficerDetailsModel.ToDate = DateTime.Parse(toDate);

                        //insertOfficerDetailsModel.FromDate = model.officerPostingDetail7.TenureFrom;
                        //insertOfficerDetailsModel.ToDate = model.officerPostingDetail7.TenureTo;

                        insertOfficerDetailsModel.actionBy = HttpContext.Session.GetString("Username");
                        insertOfficerDetailsModel.actionBy_SessionId = HttpContext.Session.Id;
                        insertOfficerDetailsModel.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

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

                        /* return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });*/ // Send the update page
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId , flag = _flag }) + "#section7");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section7");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }

        //8
        #region Added as on date 03-07-2025

        [HttpPost]
        public async Task<IActionResult> AddIntegrityAgreedOrDoubtful(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {
                string Id = HttpContext.Session.GetString("vcofficerId");
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");
                InsertIntegrityAgreedOrDoubtfulModel _insertIntegritymodel = new InsertIntegrityAgreedOrDoubtfulModel();

                try
                {
                    ModelState.Remove("officerIntegrityAgreedOrDoubtful_8.YearFrom");
                    ModelState.Remove("officerIntegrityAgreedOrDoubtful_8.YearTo");
                    ModelState.Remove("officerIntegrityAgreedOrDoubtful_8.RemovedFromAgreedlistDate");

                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerIntegrityAgreedOrDoubtful_8, nameof(model.officerIntegrityAgreedOrDoubtful_8)))
                    {
                        _insertIntegritymodel.masterReferenceId = int.Parse(MasterRefId);
                        _insertIntegritymodel.officerId = int.Parse(Id);
                        _insertIntegritymodel.enteredInTheList = model.officerIntegrityAgreedOrDoubtful_8.IsAgreed.ToString();
                        _insertIntegritymodel.dateOfEntryInTheList = DateTime.Now;
                        _insertIntegritymodel.removedFromTheList = string.Empty;
                        _insertIntegritymodel.dateOfRemovalFromTheList = string.Empty;
                        _insertIntegritymodel.actionBy = HttpContext.Session.GetString("Username");
                        _insertIntegritymodel.actionOn = DateTime.Now;
                        _insertIntegritymodel.actionBy_SessionId = HttpContext.Session.Id;
                        _insertIntegritymodel.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                        int num = await _ministry.InsertIntegrityAgreedOrDoubtful(_insertIntegritymodel);

                        if (num > 0)
                        {
                            TempData["successmsg"] = "Data Submitted Successfuly";
                            TempData.Keep("successmsg");
                        }
                        else
                        {
                            TempData["successmsg"] = "Data Not Submitted";
                        }
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section8");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section8");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }

        #endregion
        //9
        #region Added as on date 04_07_2025

        [HttpPost]
        public async Task<IActionResult> AddAllegationOfMisconductExamined(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {
                string Id = HttpContext.Session.GetString("vcofficerId");
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");
                AllegationOfMisconductExaminedModel _insertAllegationOfMisconduct = new AllegationOfMisconductExaminedModel();

                try
                {
                    ModelState.Remove("officerAllegationOfMisconductExamined_9.vigilanceAngleExamined");

                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerAllegationOfMisconductExamined_9, nameof(model.officerAllegationOfMisconductExamined_9)))
                    {
                        _insertAllegationOfMisconduct.masterReferenceId = int.Parse(MasterRefId);
                        _insertAllegationOfMisconduct.officerId = int.Parse(Id);
                        _insertAllegationOfMisconduct.vigilanceAngleExamined = "Yes";
                        _insertAllegationOfMisconduct.caseDetails = model.officerAllegationOfMisconductExamined_9.caseDetails;
                        _insertAllegationOfMisconduct.presentStatusOfTheCase = model.officerAllegationOfMisconductExamined_9.presentStatusOfTheCase;
                        _insertAllegationOfMisconduct.actionrecommendedOptions = model.officerAllegationOfMisconductExamined_9.actionrecommendedOptions;
                        _insertAllegationOfMisconduct.actionRecommendedDetails = model.officerAllegationOfMisconductExamined_9.actionRecommendedDetails;
                        _insertAllegationOfMisconduct.actionBy = HttpContext.Session.GetString("Username");
                        _insertAllegationOfMisconduct.actionOn = DateTime.Now;
                        _insertAllegationOfMisconduct.actionBy_SessionId = HttpContext.Session.Id;
                        _insertAllegationOfMisconduct.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                        int num = await _ministry.InsertAllegationOfMisconductExamined(_insertAllegationOfMisconduct);

                        if (num > 0)
                        {
                            TempData["successmsg"] = "Data Submitted Successfuly";
                            TempData.Keep("successmsg");
                        }
                        else
                        {
                            TempData["successmsg"] = "Data Not Submitted";
                        }
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section9");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId , flag = _flag }) + "#section9");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section9");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section9");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }


        //10.PunishmenetAwarded

        [HttpPost]
        public async Task<IActionResult> AddPunishmentAwarded(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {

                string Id = HttpContext.Session.GetString("vcofficerId");
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");
                PunishmentAwardedModel _punishmentAwarded = new PunishmentAwardedModel();
                try
                {
                    ModelState.Remove("officerPunishmentAwarded_10.punishmentAwarded");
                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerPunishmentAwarded_10, nameof(model.officerPunishmentAwarded_10)))
                    {
                        _punishmentAwarded.masterReferenceId = int.Parse(MasterRefId);
                        _punishmentAwarded.officerId = int.Parse(Id);
                        _punishmentAwarded.punishmentAwarded = "Yes";
                        _punishmentAwarded.punishmentDetails = model.officerPunishmentAwarded_10.punishmentDetails;
                        _punishmentAwarded.punishmentFromDate = model.officerPunishmentAwarded_10.punishmentFromDate;
                        _punishmentAwarded.punishmentToDate = model.officerPunishmentAwarded_10.punishmentToDate;
                        _punishmentAwarded.checkName_FromDate = model.officerPunishmentAwarded_10.checkName_FromDate;
                        _punishmentAwarded.checkName_ToDate = model.officerPunishmentAwarded_10.checkName_ToDate;
                        _punishmentAwarded.additionalRemarks_IfAny = model.officerPunishmentAwarded_10.additionalRemarks_IfAny;
                        _punishmentAwarded.actionBy = HttpContext.Session.GetString("Username");
                        //_punishmentAwarded.actionOn = DateTime.Now;
                        _punishmentAwarded.actionBy_SessionId = HttpContext.Session.Id;
                        _punishmentAwarded.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                        int num = await _ministry.InPunishmentAwarded(_punishmentAwarded);

                        if (num > 0)
                        {
                            TempData["successmsg"] = "Data Submitted Successfuly";
                            TempData.Keep("successmsg");
                        }
                        else
                        {
                            TempData["successmsg"] = "Data Not Submitted";
                        }
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section10");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section10");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section10");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section10");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }

        #endregion

        // 11 DisciplinaryCriminalProceedings

        #region Added as on date 07_07_2025
        [HttpPost]
        public async Task<IActionResult> AddDisciplinaryCriminalProceedings(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {
                string Id = HttpContext.Session.GetString("vcofficerId");
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");
                DisciplinaryCriminalProceedingsModel _disciplinaryCriminalProceedings = new DisciplinaryCriminalProceedingsModel();
                try
                {
                    ModelState.Remove("officerDisciplinaryCriminalProceedings_11.DisciplinaryProceeding");

                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerDisciplinaryCriminalProceedings_11, nameof(model.officerDisciplinaryCriminalProceedings_11)))
                    {
                        _disciplinaryCriminalProceedings.masterReferenceId = int.Parse(MasterRefId);
                        _disciplinaryCriminalProceedings.officerId = int.Parse(Id);
                        _disciplinaryCriminalProceedings.whether_DisciplinaryCriminalProceedingsPending = "Yes";
                        _disciplinaryCriminalProceedings.whether_Suspended = model.officerDisciplinaryCriminalProceedings_11.whether_Suspended;
                        _disciplinaryCriminalProceedings.suspensionDate = model.officerDisciplinaryCriminalProceedings_11.suspensionDate;
                        _disciplinaryCriminalProceedings.whetherRevoked = model.officerDisciplinaryCriminalProceedings_11.whetherRevoked;
                        _disciplinaryCriminalProceedings.revocationDate = model.officerDisciplinaryCriminalProceedings_11.revocationDate;
                        _disciplinaryCriminalProceedings.detailsOf_Case = model.officerDisciplinaryCriminalProceedings_11.detailsOf_Case;
                        _disciplinaryCriminalProceedings.presentStatusOftheCase = model.officerDisciplinaryCriminalProceedings_11.presentStatusOftheCase;
                        _disciplinaryCriminalProceedings.actionBy = HttpContext.Session.GetString("Username");
                        _disciplinaryCriminalProceedings.actionBy_SessionId = HttpContext.Session.Id;
                        _disciplinaryCriminalProceedings.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                        int num = await _ministry.InsertDisciplinaryCriminalProceedings(_disciplinaryCriminalProceedings);

                        if (num > 0)
                        {
                            TempData["successmsg"] = "Data Submitted Successfuly";
                            TempData.Keep("successmsg");
                        }
                        else
                        {
                            TempData["successmsg"] = "Data Not Submitted";
                        }
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section11");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section11");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section11");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section11");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }

        //12
        [HttpPost]
        public async Task<IActionResult> AddActionContemplatedAgainstTheOfficer(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {
                string Id = HttpContext.Session.GetString("vcofficerId");
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");
                ActionContemplatedAgainstTheOfficerModel _actionContemplatedAgainstTheOfficerModel = new ActionContemplatedAgainstTheOfficerModel();

                try
                {
                    ModelState.Remove("officerActionContemplatedAgainstTheOfficerAsOnDate_12.whether_CaseContemplated");

                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerActionContemplatedAgainstTheOfficerAsOnDate_12, nameof(model.officerActionContemplatedAgainstTheOfficerAsOnDate_12)))
                    {
                        _actionContemplatedAgainstTheOfficerModel.masterReferenceId = int.Parse(MasterRefId);
                        _actionContemplatedAgainstTheOfficerModel.officerId = int.Parse(Id);
                        _actionContemplatedAgainstTheOfficerModel.whether_CaseContemplated = "Yes";
                        _actionContemplatedAgainstTheOfficerModel.detailsOfTheCase = model.officerActionContemplatedAgainstTheOfficerAsOnDate_12.detailsOfTheCase;
                        _actionContemplatedAgainstTheOfficerModel.presentStatusOftheCase = model.officerActionContemplatedAgainstTheOfficerAsOnDate_12.presentStatusOftheCase;
                        _actionContemplatedAgainstTheOfficerModel.actionBy = HttpContext.Session.GetString("Username");
                        _actionContemplatedAgainstTheOfficerModel.actionBy_SessionId = HttpContext.Session.Id;
                        _actionContemplatedAgainstTheOfficerModel.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                        int num = await _ministry.InsertActionContemplatedAgainstTheOfficer(_actionContemplatedAgainstTheOfficerModel);

                        if (num > 0)
                        {
                            TempData["successmsg"] = "Data Submitted Successfuly";
                            TempData.Keep("successmsg");
                        }
                        else
                        {
                            TempData["successmsg"] = "Data Not Submitted";
                        }
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section12");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section12");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section12");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section12");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }

        #endregion

        //13
        #region Date 08_07_2025

        [HttpPost]
        public async Task<IActionResult> AddComplaintWithVigilanceAnglePending(OfficerDetailMainModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("MasterRefID")) || !string.IsNullOrEmpty(HttpContext.Session.GetString("vcofficerId")))
            {
                string Id = HttpContext.Session.GetString("vcofficerId");
                string MasterRefId = HttpContext.Session.GetString("MasterRefID");
                string _flag = HttpContext.Session.GetString("flag");                
                
                ComplaintWithVigilanceAnglePendingModel _vigilanceAnglePendingModel = new ComplaintWithVigilanceAnglePendingModel();

                try
                {
                    ModelState.Remove("officerComplaintWithVigilanceAnglePending_13.whether_vigilanceAngelPending");

                    ModelState.Clear();  // Wipe the tree
                    if (TryValidateModel(model.officerComplaintWithVigilanceAnglePending_13, nameof(model.officerComplaintWithVigilanceAnglePending_13)))
                    {
                        _vigilanceAnglePendingModel.masterReferenceId = int.Parse(MasterRefId);
                        _vigilanceAnglePendingModel.officerId = int.Parse(Id);
                        _vigilanceAnglePendingModel.whether_vigilanceAngelPending = "Yes";
                        _vigilanceAnglePendingModel.detailsOfTheCase = model.officerComplaintWithVigilanceAnglePending_13.detailsOfTheCase;
                        _vigilanceAnglePendingModel.presentStatusOftheCase = model.officerComplaintWithVigilanceAnglePending_13.presentStatusOftheCase;
                        _vigilanceAnglePendingModel.addtionalRemarks = model.officerComplaintWithVigilanceAnglePending_13.addtionalRemarks;
                        _vigilanceAnglePendingModel.actionBy = HttpContext.Session.GetString("Username");
                        _vigilanceAnglePendingModel.actionBy_SessionId = HttpContext.Session.Id;
                        _vigilanceAnglePendingModel.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                        
                        int num = await _ministry.InsertComplaintWithVigilanceAnglePending(_vigilanceAnglePendingModel);

                        if (num > 0)
                        {
                            TempData["successmsg"] = "Data Submitted Successfuly";
                            TempData.Keep("successmsg");
                        }
                        else
                        {
                            TempData["successmsg"] = "Data Not Submitted";
                        }
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section13");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section13");
                    }
                    else
                    {
                        TempData["successmsg"] = "Error: Something Went Wrong in model state";
                        TempData.Keep("successmsg");
                        //return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id }) + "#section13");
                        return Redirect(Url.Action("UpdateReferenceReceivedCVC", new { id = Id, refId = MasterRefId, flag = _flag }) + "#section13");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Failed to load country list.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogoutRedirect", "Account");
            }
        }

        #endregion


        //---------------------------------------------------------------------------------------------------------------------------
        #region Start New Reference To CVC


        [HttpGet]
        public async Task<IActionResult> New_ReferenceList()
        {
            int id = 0;
            try
            {
                //string username = HttpContext.Session.GetString("Username");
                var userDetailsJson = HttpContext.Session.GetString("UserDetails");

                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _UserName = userDetails.UserName;

                    var model = new PESBViewModel
                    {
                        new_reference_List = await _pesb.Get_Vc_Reference_Received_For_List_GetById_and_Username_Async(id, _UserName)
                    };

                    return View(model);
                }
                else
                {
                    return RedirectToAction("LogoutRedirect", "Account");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> New_Reference_FromMin_ToCVC()
        {
            try
            {
                var userDetailsJson = HttpContext.Session.GetString("UserDetails");

                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _UserName = userDetails.UserName;
                    string _Mincode = userDetails.MinCode;

                    var model = new PESBViewModel
                    {
                        reference_received_for_ddl_List = await _pesb.GetReferenceDropDownAsync(),
                        sub_post_ddl_List = new List<SelectListItem>(),
                        organization_ddl_List = await _pesb.GetOrgByMinCode(_Mincode),
                        ministry_ddl_List = new List<SelectListItem>(),
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("LogoutRedirect", "Home");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


        #region Added Code for bind the Post On change Reference 

        [HttpGet]
        public async Task<IActionResult> GetPostsByRefCode(string RefCode)
        {
            try
            {
                var RefPosts = await _pesb.GetPostDescriptionsDropDownAsync(RefCode);
                return Json(RefPosts);
            }
            catch
            {
                return Json(new { Error = "Failed to load sub-posts." });
            }
        }

        #endregion


        [HttpPost]
        public async Task<IActionResult> New_Reference_FromMin_ToCVC(PESBViewModel objmodel)
        {
            InsertReferenceReceivedForModel _insertRefModel = new InsertReferenceReceivedForModel();

            try
            {
                var userDetailsJson = HttpContext.Session.GetString("UserDetails");

                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _UserName = userDetails.UserName;

                    _insertRefModel.referenceReceivedFor = objmodel.new_reference.referenceReceivedFor;
                    _insertRefModel.referenceReceivedFrom = "Ministry";
                    _insertRefModel.referenceReceivedFromCode = objmodel.new_reference.minCode;
                    _insertRefModel.selectionForThePostCategory = objmodel.new_reference.selectionForThePostCategory ?? "";
                    _insertRefModel.selectionForThePostSubCategory = objmodel.new_reference.selectionForThePostSubCategory ?? "";
                    _insertRefModel.orgCode = objmodel.new_reference.orgCode;
                    _insertRefModel.orgName = "null";
                    _insertRefModel.minCode = objmodel.new_reference.minCode;
                    _insertRefModel.minDesc = "null";
                    _insertRefModel.pendingWith = HttpContext.Session.GetString("UserRole").ToString();
                    _insertRefModel.referenceID = objmodel.new_reference.referenceNoFileNo ?? "";
                    _insertRefModel.cvC_ReferenceID_FileNo = objmodel.new_reference.cvcReferenceIdFileNo ?? "";
                    _insertRefModel.actionBy = _UserName;
                    _insertRefModel.actionBy_SessionId = HttpContext.Session.Id;
                    _insertRefModel.actionBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString();

                    int isInserted = await _ministry.Insert_New_Reference(_insertRefModel);

                    if (isInserted > 0)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, error_new_reference_message = "This record is not saved." });
                    }
                }

                else
                {
                    return RedirectToAction("LogoutRedirect", "Home");
                }

            }
            catch (Exception ex)
            {
                return View("New_Reference_FromMin_ToCVC");
            }
        }


        [HttpGet]
        public async Task<IActionResult> New_Reference_Details(int MasterRefID)
        {
            try
            {
                TempData["RefId"] = MasterRefID;
                var result = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(MasterRefID);

                var model = new AddNewOfficerMainModel
                {
                    new_Reference_Model = result.FirstOrDefault(),
                    officerList = await _ministry.GetOfficerListAsync(MasterRefID.ToString()),
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
        public async Task<IActionResult> AddNewOfficerDetails(int id)
        {
            try
            {
                var data = new InsertNewOfficerDetailFromMinistryModel
                {
                    masterReferenceID = id
                };

                var model = new AddNewOfficerMainModel
                {
                    service_ddl_List = await _pesb.Get_Service_DropDownAsync(),
                    batch_ddl_List = await _pesb.Get_Batch_DropDownAsync(),
                    cadre_ddl_List = await _pesb.Get_Cadre_DropDownAsync(),
                    insertNewOfficerDetailFromMinistryModel = data,
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
        public async Task<IActionResult> AddNewOfficerDetails([FromBody] AddNewOfficerMainModel objmodel)
        {
            try
            {

                var userDetailsJson = HttpContext.Session.GetString("UserDetails");

                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _UserName = userDetails.UserName;

                    var _service = objmodel.insertNewOfficerDetailFromMinistryModel.officer_Service;
                    var Otherservice = string.Empty;
                    var _Cadre = string.Empty;

                    if (_service == "OTHERS")
                        Otherservice = objmodel.insertNewOfficerDetailFromMinistryModel.officer_Other_Service;
                    else
                        objmodel.insertNewOfficerDetailFromMinistryModel.officer_Other_Service = "";

                    if (_service == "IPS" || _service == "IAS" || _service == "IFoS")
                        _Cadre = objmodel.insertNewOfficerDetailFromMinistryModel.officer_Cadre;
                    else
                        objmodel.insertNewOfficerDetailFromMinistryModel.officer_Cadre = "";

                    // objmodel.insertNewOfficerDetailFromMinistryModel.masterReferenceID = MasterRefId;
                    objmodel.insertNewOfficerDetailFromMinistryModel.createdBy = _UserName;
                    objmodel.insertNewOfficerDetailFromMinistryModel.createdBy_SessionId = HttpContext.Session.Id;
                    objmodel.insertNewOfficerDetailFromMinistryModel.createdBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                    int num = await _ministry.Insert_New_Officer_Details(objmodel.insertNewOfficerDetailFromMinistryModel);
                    if (num == 0)
                    {
                        return Json(new { success = false, message = "Record not saved." });
                    }

                    return Json(new
                    {
                        success = true,

                        redirectUrl = Url.Action("New_Reference_Details", new { MasterRefID = objmodel.insertNewOfficerDetailFromMinistryModel.masterReferenceID })

                    });

                    //return RedirectToAction();
                }

                else
                {
                    return RedirectToAction("LogoutRedirect", "Home");
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Unexpected error occurred." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ForwardToApprover(int id ,string flag)
        {
            try
            {
                string _pendingWith = string.Empty;
                if (flag == "APPROVER")
                {
                    _pendingWith = "MINISTRY_APPROVER";
                }
                else if (flag == "CVO")
                {
                    _pendingWith = "MINISTRY_CVO";
                }
                else if (flag == "COORD")
                {
                    _pendingWith = "CVC_COORD2_DH";
                }

                var userDetailsJson = HttpContext.Session.GetString("UserDetails");

                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _UserName = userDetails.UserName;

                    UpdateRefFromPESBModel _updateRefModel = new UpdateRefFromPESBModel();

                    _updateRefModel.mainReferenceID = id;
                    _updateRefModel.pendingWith = _pendingWith;
                    _updateRefModel.updatedBy = _UserName;
                    _updateRefModel.updatedBy_IP = HttpContext.Connection.RemoteIpAddress?.ToString();
                    _updateRefModel.updatedBy_SessionId = HttpContext.Session.Id;

                    int num = await _ministry.UpdateReferenceFromPESB(_updateRefModel);
                    if (num == 0)
                    {
                        //return RedirectToAction("New_ReferenceList");
                        TempData["msg"] = "Forward to Successfully";
                    }
                    else
                    {
                        //return RedirectToAction("New_ReferenceList");
                        TempData["msg"] = "Error: Something went wrong Please try again or contact system Admin";
                    }

                    if (flag == "APPROVER")
                    {
                        return RedirectToAction("New_ReferenceList");
                    }
                    else if (flag == "CVO" || flag == "COORD")
                    {
                        return RedirectToAction("GetReferenceListPendingWith_MinistryApprover");
                    }
                    else
                    {
                        return RedirectToAction("New_ReferenceList"); // Chnage karna hai ministry CVO Login me redirect karna
                    }
                }
                else
                {
                    return RedirectToAction("LogoutRedirect", "Account");
                }
                //return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        #endregion


        #region Start the code of ministry Approver as on date 22_07_2025

        [HttpGet]
        public async Task<IActionResult> GetReferenceListPendingWith_MinistryApprover()
        {
            try
            {
                var userDetailsJson = HttpContext.Session.GetString("UserDetails");
                if (!string.IsNullOrEmpty(userDetailsJson))
                {
                    var _Role = HttpContext.Session.GetString("UserRole");

                    var userDetails = System.Text.Json.JsonSerializer.Deserialize<UserDetailsModel>(userDetailsJson);
                    string _MinMastercode = userDetails.MinMasterCode;
                    string _MinCode = userDetails.MinCode;

                   // List<ReferenceReceivedFromCVCModel> apiResponse = await _ministry.GetReferenceReceivedFromCVClist(_MinMastercode);
                    List<ReferenceListPendingWith_MinistryApproverModel> apiResponse = await _ministry.GetReferenceListPendingWith_MinistryApproverList(_MinCode, _Role);
                    if (apiResponse != null)
                    {
                        return View(apiResponse);
                    }

                    return View(new List<ReferenceReceivedFromCVCModel>());
                }
                else
                {
                    return RedirectToAction("LogoutRedirect", "Account");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        #endregion


    }
}
