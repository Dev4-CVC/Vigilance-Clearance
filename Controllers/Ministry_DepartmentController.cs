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
using VigilanceClearance.Models.Modal_Properties;
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
            try
            {
                string _UserName = "minpower";

                List<ReferenceReceivedFromCVCModel> apiResponse = await _ministry.GetReferenceReceivedFromCVClist(_UserName);

                if (apiResponse != null)
                {                   
                    return View(apiResponse);
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
        public async Task<IActionResult> UpdateReferenceReceivedCVC(string id)
        {
            try
            {
                HttpContext.Session.SetString("vcofficerId", id);
               
                List<OfficerListModel> Response = await _ministry.GetOfficerListAsync(id);

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
                List<DisciplinaryCriminalProceedingsModel> DisciplinaryCriminalProceedingsModellist =  await _ministry.GetDisciplinaryCriminalProceedingsModelList(id);

                //12
                List<ActionContemplatedAgainstTheOfficerModel> ActionContemplatedAgainstTheOfficerlist = await _ministry.GetActionContemplatedAgainstTheOfficerlList(id);

                //13
                List<ComplaintWithVigilanceAnglePendingModel> ComplaintWithVigilanceAnglePendingModellist = await _ministry.GetComplaintWithVigilanceAnglePendingList(id);

                

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
                    DisciplinaryCriminalProceedingsModellist= DisciplinaryCriminalProceedingsModellist,
                    ActionContemplatedAgainstTheOfficerModellist = ActionContemplatedAgainstTheOfficerlist,
                    ComplaintWithVigilanceAnglePendingModellist = ComplaintWithVigilanceAnglePendingModellist,
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
            InsertOfficerDetailsModel insertOfficerDetailsModel = new InsertOfficerDetailsModel();
            string Id = HttpContext.Session.GetString("vcofficerId");
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

                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id }); // Send the update page
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }


        #region Added as on date 03-07-2025


        [HttpPost]
        public async Task<IActionResult> AddIntegrityAgreedOrDoubtful(OfficerDetailMainModel model)
        {
            string Id = HttpContext.Session.GetString("vcofficerId");

            InsertIntegrityAgreedOrDoubtfulModel _insertIntegritymodel = new InsertIntegrityAgreedOrDoubtfulModel();

            try
            {
                ModelState.Remove("officerIntegrityAgreedOrDoubtful_8.YearFrom");
                ModelState.Remove("officerIntegrityAgreedOrDoubtful_8.YearTo");
                ModelState.Remove("officerIntegrityAgreedOrDoubtful_8.RemovedFromAgreedlistDate");

                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerIntegrityAgreedOrDoubtful_8, nameof(model.officerIntegrityAgreedOrDoubtful_8)))
                {

                    _insertIntegritymodel.officerId =int.Parse(Id);
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

                   // return RedirectToAction("UpdateReferenceReceivedCVC", Id); // Send the update page
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }

        #endregion

        #region Added as on date 04_07_2025

        [HttpPost]
        public async Task<IActionResult> AddAllegationOfMisconductExamined(OfficerDetailMainModel model)
        {
            string Id = HttpContext.Session.GetString("vcofficerId");
           
            AllegationOfMisconductExaminedModel _insertAllegationOfMisconduct = new AllegationOfMisconductExaminedModel();

            try
            {
                
                ModelState.Remove("officerAllegationOfMisconductExamined_9.vigilanceAngleExamined");

                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerAllegationOfMisconductExamined_9, nameof(model.officerAllegationOfMisconductExamined_9)))
                {

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

                    // return RedirectToAction("UpdateReferenceReceivedCVC", Id); // Send the update page
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }


        //10.PunishmenetAwarded

        [HttpPost]
        public async Task<IActionResult> AddPunishmentAwarded(OfficerDetailMainModel model)
        {
            string Id = HttpContext.Session.GetString("vcofficerId");

            PunishmentAwardedModel _punishmentAwarded = new PunishmentAwardedModel();

            try
            {

                ModelState.Remove("officerPunishmentAwarded_10.punishmentAwarded");

                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerPunishmentAwarded_10, nameof(model.officerPunishmentAwarded_10)))
                {

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
                  
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }

        #endregion

        // 11 DisciplinaryCriminalProceedings

        #region Added as on date 07_07_2025
        [HttpPost]
        public async Task<IActionResult> AddDisciplinaryCriminalProceedings(OfficerDetailMainModel model)
        {
            string Id = HttpContext.Session.GetString("vcofficerId");

            //PunishmentAwardedModel _punishmentAwarded = new PunishmentAwardedModel();

            DisciplinaryCriminalProceedingsModel _disciplinaryCriminalProceedings = new DisciplinaryCriminalProceedingsModel();
            try
            {

                ModelState.Remove("officerDisciplinaryCriminalProceedings_11.DisciplinaryProceeding");

                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerDisciplinaryCriminalProceedings_11, nameof(model.officerDisciplinaryCriminalProceedings_11)))
                {

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

                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddActionContemplatedAgainstTheOfficer(OfficerDetailMainModel model)
        {
            string Id = HttpContext.Session.GetString("vcofficerId");

            ActionContemplatedAgainstTheOfficerModel _actionContemplatedAgainstTheOfficerModel = new ActionContemplatedAgainstTheOfficerModel();

            try
            {
                ModelState.Remove("officerActionContemplatedAgainstTheOfficerAsOnDate_12.whether_CaseContemplated");

                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerActionContemplatedAgainstTheOfficerAsOnDate_12, nameof(model.officerActionContemplatedAgainstTheOfficerAsOnDate_12)))
                {

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

                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }

        #endregion

        #region Date 08_07_2025

        [HttpPost]
        public async Task<IActionResult> AddComplaintWithVigilanceAnglePending(OfficerDetailMainModel model)
        {
            string Id = HttpContext.Session.GetString("vcofficerId");

            //ActionContemplatedAgainstTheOfficerModel _actionContemplatedAgainstTheOfficerModel = new ActionContemplatedAgainstTheOfficerModel();

            ComplaintWithVigilanceAnglePendingModel _vigilanceAnglePendingModel = new ComplaintWithVigilanceAnglePendingModel();

            try
            {
                ModelState.Remove("officerComplaintWithVigilanceAnglePending_13.whether_vigilanceAngelPending");

                ModelState.Clear();  // Wipe the tree
                if (TryValidateModel(model.officerComplaintWithVigilanceAnglePending_13, nameof(model.officerComplaintWithVigilanceAnglePending_13)))
                {
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

                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
                else
                {
                    TempData["successmsg"] = "Error: Something Went Wrong in model state";
                    TempData.Keep("successmsg");
                    return RedirectToAction("UpdateReferenceReceivedCVC", new { id = Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View();
            }
        }

        #endregion 

    }
}
