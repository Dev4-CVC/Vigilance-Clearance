
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using VigilanceClearance.Interface.Admin;
using VigilanceClearance.Models.Admin;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.PESB;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models;
using System.Net;

namespace VigilanceClearance.DataAccessLayer.Admin_Service
{
    public class Admin_Service : IAdmin
    {
        private readonly string APIURL;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        public Admin_Service(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            APIURL = configuration["APIURL"]!;
            this._httpContextAccessor = httpContextAccessor;
            this._httpClient = clientFactory.CreateClient();
        }

        public async Task<List<Users_Model>> Get_Users_List_Async()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return new List<Users_Model>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}AdminSec/get-users");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API failure: {response.StatusCode} | {error}");
                }


                var jsonContent = await response.Content.ReadAsStringAsync();
                var usersList = JsonConvert.DeserializeObject<List<Users_Model>>(jsonContent);
                return usersList ?? new List<Users_Model>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get_Users_List_Async: {ex.Message}");
            }

            return new List<Users_Model>();
        }
        public async Task<List<SelectListItem>> Get_Roles_Dropdown_Async()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}AdminSec/get-role");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<SelectListItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<string>>(json) ?? new();

                return roles.Select(role => new SelectListItem
                {
                    Value = role,
                    Text = role
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }
        public async Task<int> Insert_Add_New_User_Async(Users_Model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}AdminSec/register";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                objmodel.roles = new List<string> { objmodel.Roles };
                var json = System.Text.Json.JsonSerializer.Serialize(objmodel, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);
                
                
                var responseContent = await response.Content.ReadAsStringAsync();

                if (responseContent.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    return -1;
                }

                // ✅ Success
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(responseJson, out int result))
                        return result;

                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return 0;
            }
        }



        public async Task<List<UsersRole_Model>> Get_Users_Role_List_Async()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
                return new List<UsersRole_Model>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}AdminSec/get-role");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API failure: {response.StatusCode} | {error}");
                }
                var jsonContent = await response.Content.ReadAsStringAsync();
                var roleNames = JsonConvert.DeserializeObject<List<string>>(jsonContent);

                // Map strings to UsersRole_Model
                var usersList = roleNames.Select(role => new UsersRole_Model { RoleName = role }).ToList();
                return usersList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get_Users_Role_List_Async: {ex.Message}");
            }

            return new List<UsersRole_Model>();
        }
        public async Task<int> Insert_Add_New_User_Role_Async(UsersRole_Model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}AdminSec/add-role";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(objmodel.RoleName, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    return 0;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                if (errorContent.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                {
                    return -1; // Duplicate entry
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                if (int.TryParse(responseJson, out int result))
                    return result;

                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return 0;
            }
        }
    }
}
