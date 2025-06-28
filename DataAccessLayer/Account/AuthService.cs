using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using VigilanceClearance.Interface.Account;
using VigilanceClearance.Models.DTOs;
using VigilanceClearance.Models.Modal_Properties.Account;

namespace VigilanceClearance.Data.Account
{
    public class AuthService : IAuthService
    {
        private readonly string APIURL;
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            APIURL = configuration["APIURL"]!;
            this._httpClient = httpClient;
        }

        public async Task<TokenResponse?> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{APIURL}Auth/login", loginDto);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse>(content);
        }
    }
}
