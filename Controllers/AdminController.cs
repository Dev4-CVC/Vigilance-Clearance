using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.CodeDom;
using VigilanceClearance.Interface.Admin;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models.Admin;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IAdmin _admin;
        public AdminController(IHttpClientFactory clientFactory, HttpClient httpClient, IConfiguration configuration, IAdmin admin)
        {
            this._clientFactory = clientFactory;
            this._httpClient = httpClient;
            this._configuration = configuration;
            this._admin = admin;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Users()
        {
            ViewBag.title = "Users";
            
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = new UsersMain_Model
                {
                    users_List = await _admin.Get_Users_List_Async()
                };

                var successMessage = TempData["SuccessMessage"] as string;
                if (!string.IsNullOrEmpty(successMessage))
                {
                    ViewBag.SuccessMessage = successMessage;
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
        public async Task<IActionResult> AddUsers()
        {
            ViewBag.title = "Users";
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                var model = new UsersMain_Model
                {
                    roles_ddl_List = await _admin.Get_Roles_Dropdown_Async(),
                    users = new Users_Model()
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
        public async Task<IActionResult> AddUsers(UsersMain_Model objmodel)
        {
            ViewBag.title = "Users Role";

            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                int isInserted = await _admin.Insert_Add_New_User_Async(objmodel.users);

                if (isInserted == -1)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email or username already exists.");
                    return View(objmodel);
                }

                if (isInserted > 0)
                {
                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToAction("Users"); // adjust to your user list view
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to save user.");
                    return View(objmodel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server error occurred.");
                return View(objmodel);
            }
        }




        [HttpGet]
        public async Task<IActionResult> UsersRole()
        {
            ViewBag.title = "Users Role";
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = new UsersMain_Model
                {
                    users_role_List = await _admin.Get_Users_Role_List_Async(),
                    users_role = new UsersRole_Model()
                };


                var successMessage = TempData["SuccessMessage"] as string;
                if (!string.IsNullOrEmpty(successMessage))
                {
                    ViewBag.SuccessMessage = successMessage;
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
        public async Task<IActionResult> AddUsersRole()
        {
            ViewBag.title = "Users Role";
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = new UsersMain_Model
                {
                    users_role = new UsersRole_Model()
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
        public async Task<IActionResult> AddUsersRole(UsersMain_Model objmodel)
        {
            ViewBag.title = "Users Role";

            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                int isInserted = await _admin.Insert_Add_New_User_Role_Async(objmodel.users_role);

                if (isInserted == -1)
                {
                    ModelState.AddModelError(string.Empty, "Role already exists.");
                    return View(objmodel);
                }
                if (isInserted > 0)
                {
                    TempData["SuccessMessage"] = "Role created successfully!";
                    return RedirectToAction("UsersRole"); // or your target action
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to save role.");
                    return View(objmodel);
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error occurred.");
                return View(objmodel);
            }
        }





    }
}
