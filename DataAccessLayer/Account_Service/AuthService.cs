using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VigilanceClearance.Interface.Account;
using VigilanceClearance.Models.DTOs;
using VigilanceClearance.Models.Modal_Properties.Account;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.Ministry;

namespace VigilanceClearance.Data.Account
{
    public class AuthService : IAuthService
    {
        private readonly string APIURL;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            APIURL = configuration["APIURL"]!;
            this._httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponse?> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{APIURL}Auth/login", loginDto);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(content);
        }


        public async Task<UserDetailsModel> GetUserDetailsbyUserName(string _UserName)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new UserDetailsModel();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}User/GetUserDetails?UserName={Uri.EscapeDataString(_UserName)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new UserDetailsModel();
                }
                var jsonContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<OfficerListAPIModel>(jsonContent);
                //var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<UserDetailsModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                    //return new UserDetailsModel();
                }

                return new UserDetailsModel();
            }
            catch (Exception)
            {
                return new UserDetailsModel();
            }
        }

    }
}
