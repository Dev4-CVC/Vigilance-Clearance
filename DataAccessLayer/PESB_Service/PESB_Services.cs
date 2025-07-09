using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models;
using VigilanceClearance.Models.Modal_Properties.PESB;

namespace VigilanceClearance.DataAccessLayer.PESB_Service
{
    public class PESB_Services : IPESB
    {
        private readonly string APIURL;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public PESB_Services(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            APIURL = configuration["APIURL"]!;
            this._httpContextAccessor = httpContextAccessor;
            this._httpClient = clientFactory.CreateClient();
        }

        //db table name: tbl_Master_Vc_Reference
        public async Task<List<SelectListItem>> GetReferenceDropDownAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}DropDown/VcReferenceGetAll");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<SelectListItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new();

                return items.Select(item => new SelectListItem
                {
                    Value = item.Value,
                    Text = item.Text
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }


        //db table name: tbl_Master_Vc_Post
        public async Task<List<SelectListItem>> GetPostDescriptionsDropDownAsync(string ReferenceCode)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}DropDown/VcPostGetByReferenceNumber?ReferenceNumber={Uri.EscapeDataString(ReferenceCode)}";
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<SelectListItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(json) ?? new List<DropDownResponseModel>();

                return items.Select(item => new SelectListItem
                {
                    Value = item.Value,
                    Text = item.Text
                }).ToList();
            }
            catch (Exception)
            {
                return new List<SelectListItem>();
            }
        }


        //db table name: tbl_Master_Vc_Post_SubCategory
        public async Task<List<SelectListItem>> GetSubPostsByPostCodeInternal(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode)) return new List<SelectListItem>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}DropDown/VcPostSubCategoryGetById?Id={Uri.EscapeDataString(postCode)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new List<SelectListItem>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(jsonContent) ?? new List<DropDownResponseModel>();

                return items.Select(item => new SelectListItem
                {
                    Value = item.Value,
                    Text = item.Text
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }


        //db table name: tbl_Master_Vc_MinistryNew
        public async Task<List<SelectListItem>> GetOrganizationDropDownAsync(string id = "0")
        {
            if (string.IsNullOrWhiteSpace(id)) return new List<SelectListItem>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string requestUrl = $"{APIURL}DropDown/MinistryNewGetBySection?section={Uri.EscapeDataString(id)}";
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new List<SelectListItem>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(jsonContent) ?? new List<DropDownResponseModel>();

                return items.Select(item => new SelectListItem
                {
                    Value = item.Value,
                    Text = item.Text
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }


        //db table name: tbl_Master_Vc_MinistryNew
        public async Task<List<SelectListItem>> GetMinistryByPostCodeInternal(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return new List<SelectListItem>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}DropDown/GetMinistryByOrgCode?orgcode={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new List<SelectListItem>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<DropDownResponseModel>>(jsonContent) ?? new List<DropDownResponseModel>();

                return items.Select(item => new SelectListItem
                {
                    Value = item.Value,
                    Text = item.Text
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }




        public async Task<int> InsertAddNewReferenceAsync(PESB_Add_New_Reference_Model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedForInsert";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(objmodel, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    return 0;
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                if (int.TryParse(responseJson, out int result))
                    return result;

                // Optionally handle more structured API response if needed
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
