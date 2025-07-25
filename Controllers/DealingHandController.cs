using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Interface.Dealing_Hand;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.Dealing_Hand;
using VigilanceClearance.Models.Modal_Properties;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.ViewModel.Ministry;
using VigilanceClearance.Models.ViewModel.PESB;
using static System.Collections.Specialized.BitVector32;

namespace VigilanceClearance.Controllers
{
    public class DealingHandController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IDealingHand _dealinghand;
        private readonly IMinistry _ministry;
        private readonly IPESB _pesb;
        public DealingHandController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration, IDealingHand dealinghand, IMinistry ministry, IPESB pesb)
        {
            this._clientFactory = clientFactory;
            this._httpClient = httpClient;
            this._configuration = configuration;
            this._dealinghand = dealinghand;
            _ministry = ministry;
            _pesb = pesb;
        }

        [HttpGet]
        public async Task<IActionResult> DH_Dashboard()
        {
            ViewBag.title = "DH Dashboard";
            try
            {
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Pending_References_With_Branches()
        {
            ViewBag.title = "Pending_References_With_Branches";
            string section = HttpContext.Session.GetString("Section");
            try
            {
                var model = new Dealing_Hand_View_Model
                {
                    pending_list_for_branch_DH_list = await _dealinghand.Get_PendingListFor_BranchDH_Async(section),
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
        public async Task<IActionResult> Pending_Officers(int id) 
        {
            ViewBag.title = "Pending_Officers";
            string section = HttpContext.Session.GetString("Section");
            HttpContext.Session.SetInt32("MasterRefId", id);
            try
            {
                var model = new Dealing_Hand_View_Model
                {
                    pending_officer_details_for_cvc_users_list = await _dealinghand.Get_Pending_Officer_Details_For_CVC_Users_Async(id, section),
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
        public async Task<IActionResult> DealingHand_Officer_CompleteProfile(int id)
        {
            int MasterRefId = (int)HttpContext.Session.GetInt32("MasterRefId");

            string vcOfficerId = id.ToString();

            try
            {
               
                TempData["RefId"] = id;  // Iska use back button ke liye ho raha hai / agar first box se aana hai to refid jayegi or second se aaya hai to id (Matlab officerid) jayegi

                List<OfficerListModel> Response = await _ministry.GetOfficerDetailsByOfficerIdAsync(vcOfficerId);

                OfficerListModel firstOfficer = Response.FirstOrDefault();
                //7
                List<OfficerPostingDetailsViewModellist> Postinglist = await _ministry.GetOfficerPostingList(vcOfficerId);
                //8
                List<InsertIntegrityAgreedOrDoubtfulModel> IntegrityAgreedlist = await _ministry.GetInsertIntegrityAgreedOrDoubtfulList(vcOfficerId);

                //9           
                List<AllegationOfMisconductExaminedModel> AllegationOfMisconductlist = await _ministry.GetAllegationOfMisconductExaminedList(vcOfficerId);

                //10
                List<PunishmentAwardedModel> PunishmentAwardedlist = await _ministry.GetPunishmentAwardedList(vcOfficerId);

                //11 
                List<DisciplinaryCriminalProceedingsModel> DisciplinaryCriminalProceedingsModellist = await _ministry.GetDisciplinaryCriminalProceedingsModelList(vcOfficerId);

                //12
                List<ActionContemplatedAgainstTheOfficerModel> ActionContemplatedAgainstTheOfficerlist = await _ministry.GetActionContemplatedAgainstTheOfficerlList(vcOfficerId);

                //13
                List<ComplaintWithVigilanceAnglePendingModel> ComplaintWithVigilanceAnglePendingModellist = await _ministry.GetComplaintWithVigilanceAnglePendingList(vcOfficerId);


                var result = await _pesb.Get_Vc_Reference_Received_For_Details_GetById_Async(MasterRefId);


                string orgcode = string.Empty;

                var model = new OfficerDetailMainModel
                {
                    officerPostingDetail7 = new OfficerPostingDetails
                    {
                        OrganizationList = await _ministry.GetOrganizationDropDownAsync("0"),
                        Organization = null // or default
                    },

                    officerPersonalDetailModel = new Models.OfficerDetailModel.OfficerPersonalDetailModel
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
        public async Task<IActionResult> AddVigBranchComments(VigBranchCommentsInsert obj)
        {
            ViewBag.title = "Vig Branch Comments";
            string section = HttpContext.Session.GetString("Section");

            int MasterRefId = (int)HttpContext.Session.GetInt32("MasterRefId");
            try
            {
               
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }


    }
}
