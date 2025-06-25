using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.NewReferenceToCVC;

namespace VigilanceClearance.Controllers
{
    public class Ministry_New_ReferenceController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Ministry_New_ReferenceController(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public async Task<IActionResult> Ministry_New_Reference_Dashboard()
        {
            try
            {
                ViewBag.title = "Dashboard";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }

        public async Task<IActionResult> NewReferenceToCVC()
        {
            try
            {
                ViewBag.title = "New Reference To CVC";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }



        public async Task<IActionResult> ReferenceReceivedFromCVC()
        {
            try
            {
                ViewBag.title = "Reference Received Fro CVC";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }


        public async Task<IActionResult> AddNewReference()
        {
            try
            {
                ViewBag.title = "Add New Reference";

                var model = new NewReferenceToCVCViewModel
                {
                    EmpanelmentList = await GetEmpanelmentAsync(),
                    AppointmentList = await GetAppointmentAsync()
                };

                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return View();
            }
        }

        private Task<List<SelectListItem>> GetEmpanelmentAsync()
        {
            return Task.FromResult(new List<SelectListItem>
            {
                new SelectListItem { Value = "Joint Secretary", Text = "JointSecretary" },
                new SelectListItem { Value = "Additional Secretary", Text = "Additional Secretary" },
                new SelectListItem { Value = "Secretary", Text = "Secretary" },
                new SelectListItem { Value = "General Manager", Text = "General Manager" },
                new SelectListItem { Value = "Executive Director", Text = "Executive Director" },
                new SelectListItem { Value = "Deputy Inspector General", Text = "Deputy Inspector General" },
                new SelectListItem { Value = "Inspector General", Text = "Inspector General" },
                new SelectListItem { Value = "Additional Director General", Text = "Additional Director General" },
                new SelectListItem { Value = "Director General", Text = "Director General" }
            });
        }
        private Task<List<SelectListItem>> GetAppointmentAsync()
        {
            return Task.FromResult(new List<SelectListItem>
            {
                new SelectListItem { Value = "CHAIRMAN", Text = "CHAIRMAN" },
                new SelectListItem { Value = "CHAIRMAN and MD", Text = "CHAIRMAN and MD" },
                new SelectListItem { Value = "MANAGING DIRECTOR", Text = "MANAGING DIRECTOR" },
                new SelectListItem { Value = "DIRECTOR", Text = "DIRECTOR" }
            });
        }



        public async Task<IActionResult> Empanelment()
        {
            try
            {
                ViewBag.title = "Empanelment";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }
        public async Task<IActionResult> Appointment()
        {
            try
            {
                ViewBag.title = "Appointment";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }
        public async Task<IActionResult> Extension()
        {
            try
            {
                ViewBag.title = "Extension";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }
        public async Task<IActionResult> AdditionalCharge()
        {
            try
            {
                ViewBag.title = "AdditionalCharge";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }
        public async Task<IActionResult> Confirmation()
        {
            try
            {
                ViewBag.title = "Confirmation";
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong while loading the page.";
                return await Task.FromResult(View());
            }
        }

    }
}
