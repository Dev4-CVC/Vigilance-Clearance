using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Models;
using VigilanceClearance.Models.OfficerDetailModel;

namespace VigilanceClearance.DataAccessLayer.Ministry_Service
{
    public class Ministry_Service : IMinistry
    {
        private readonly string BaseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public Ministry_Service(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            BaseUrl = configuration["BaseUrl"]!;
            this._httpContextAccessor = httpContextAccessor;
            this._httpClient = clientFactory.CreateClient();
        }



        public async Task<List<SelectListItem>> GetOrganizationDropDownAsync(string section)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}DropDown/MinistryNewGetBySection?section={Uri.EscapeDataString(section)}");

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
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }


        public async Task<List<SelectListItem>> GetMinistryDropDownbycodeAsync(string orgcode)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{BaseUrl}DropDown/GetMinistryByOrgCode?orgcode={Uri.EscapeDataString(orgcode)}");

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
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

        public async Task<int> InsertOfficerPostingDetail(InsertOfficerDetailsModel objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedForInsert";
                var requestUrl = $"{BaseUrl}OfficerPostingDetails/addOfficerPostingDetails";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(objmodel, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);

                //var response = await client.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    return 0;
                }

                var responseJson = await response.Content.ReadAsStringAsync();


                if (int.TryParse(responseJson, out int result))
                {
                    return result;
                }


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
