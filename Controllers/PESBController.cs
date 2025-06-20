using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.ViewModel;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mono.TextTemplating;
using System;

namespace VigilanceClearance.Controllers
{
    public class PESBController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PESBController(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public async Task<IActionResult> PESBDashboard()
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
                ViewBag.title = "PESB Report Form";
                ViewBag.pagetitle = "PESB : Add New Reference";
                var model = new PESBViewModel
                {
                    PostDescriptionList = await GetPostDescriptionsAsync(),
                    SubPostDescriptionList = await GetSubPostDescriptionsAsync(),
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

        private Task<List<SelectListItem>> GetPostDescriptionsAsync()
        {
            return Task.FromResult(new List<SelectListItem>
            {
                new SelectListItem { Value = "VC101", Text = "Vigilance Clearance OFFICER" },
                new SelectListItem { Value = "CHAIRMAN", Text = "CHAIRMAN" },
                new SelectListItem { Value = "DIRECTOR", Text = "DIRECTOR" },
                new SelectListItem { Value = "CRO", Text = "CHIEF RESEARCH Officer" }
            });
        }

        private Task<List<SelectListItem>> GetSubPostDescriptionsAsync()
        {
            return Task.FromResult(new List<SelectListItem>
            {
                new SelectListItem { Value = "TECH", Text = "TECHNICAL" },
                new SelectListItem { Value = "HR", Text = "HR" },
                new SelectListItem { Value = "OPERATIONS", Text = "OPERATIONS" },
                new SelectListItem { Value = "COMMERCIAL", Text = "COMMERCIAL" },
                new SelectListItem { Value = "MARKETING", Text = "MARKETING" },
                new SelectListItem { Value = "BD", Text = "BUSINESS DEVELOPMENT" },
                new SelectListItem { Value = "GREENENERGY", Text = "GREENENERGY" },
                new SelectListItem { Value = "ceo102", Text = "ceo102" },
                new SelectListItem { Value = "Other", Text = "Other" },
            });
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
                    model.PostDescriptionList = await GetPostDescriptionsAsync();
                    model.SubPostDescriptionList = await GetSubPostDescriptionsAsync();
                    return View(model);
                }
                return RedirectToAction("PESBDashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An unexpected error occurred. Please try again.";
                model.PostDescriptionList = await GetPostDescriptionsAsync();
                model.SubPostDescriptionList = await GetSubPostDescriptionsAsync();
                return View(model);
            }
        }







        [HttpGet]
        public async Task<IActionResult> CountryData()
        {
            var client = _clientFactory.CreateClient();

            try
            {
                var countries = await client.GetFromJsonAsync<List<CountryModel>>("https://restcountries.com/v3.1/all?fields=name,cca2");

                var model = new CountryViewModel
                {
                    CountryList = countries.OrderBy(c => c.Name.Common).Select(c => new SelectListItem
                    {
                        Text = c.Name.Common,
                        Value = c.Cca2
                    }).ToList()
                };
                return View(model);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load country list.");
                return View(new CountryViewModel { CountryList = new List<SelectListItem>() });
            }
        }

    }
}
