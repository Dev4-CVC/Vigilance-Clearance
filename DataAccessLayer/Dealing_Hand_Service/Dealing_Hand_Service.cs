using Newtonsoft.Json;
using System.Net.Http.Headers;
using VigilanceClearance.Interface.Dealing_Hand;
using VigilanceClearance.Models.Dealing_Hand;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.Ministry;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.DataAccessLayer.Dealing_Hand_Service
{
    public class Dealing_Hand_Service : IDealingHand
    {
        private readonly string BaseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public Dealing_Hand_Service(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            BaseUrl = configuration["BaseUrl"]!;
            this._httpContextAccessor = httpContextAccessor;
            this._httpClient = clientFactory.CreateClient();
        }

        public async Task<List<PendingListFor_BranchDH>> Get_PendingListFor_BranchDH_Async(string branch)
        {
            if (string.IsNullOrWhiteSpace(branch)) return new List<PendingListFor_BranchDH>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<PendingListFor_BranchDH>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Assuming the endpoint expects a branch as id
            var requestUrl = $"{BaseUrl}DealingHand/ReferencesPendingWithDealingHand?branch={branch}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<PendingListFor_BranchDH>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<PendingListFor_BranchDH>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }

                return new List<PendingListFor_BranchDH>();
            }
            catch (Exception ex)
            {
                return new List<PendingListFor_BranchDH>();
            }
        }


        public async Task<List<PendingOfficerDetailsForCVCUsers>> Get_Pending_Officer_Details_For_CVC_Users_Async(int id, string branch)
        {
            if (string.IsNullOrWhiteSpace(branch)) return new List<PendingOfficerDetailsForCVCUsers>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<PendingOfficerDetailsForCVCUsers>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Assuming the endpoint expects a branch as id
            var requestUrl = $"{BaseUrl}DealingHand/OfficerDetailsForCVCUsers?masterRefid={id}&branch={branch}";


            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<PendingOfficerDetailsForCVCUsers>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<PendingOfficerDetailsForCVCUsers>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }

                return new List<PendingOfficerDetailsForCVCUsers>();
            }
            catch (Exception ex)
            {
                return new List<PendingOfficerDetailsForCVCUsers>();
            }
        }


    }
}
