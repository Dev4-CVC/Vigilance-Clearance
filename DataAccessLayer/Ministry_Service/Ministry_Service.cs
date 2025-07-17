using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VigilanceClearance.Interface.Ministry;
using VigilanceClearance.Models;
using VigilanceClearance.Models.Modal_Properties;
using VigilanceClearance.Models.New_Reference_to_CVCModels;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel;
using VigilanceClearance.Models.ViewModel.Ministry;

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


        public async Task<List<ReferenceReceivedFromCVCModel>> GetReferenceReceivedFromCVClist(string _UserName)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<ReferenceReceivedFromCVCModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}VcReferenceReceivedFor/VcReferenceReceivedForGetById?UserName={Uri.EscapeDataString(_UserName)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<ReferenceReceivedFromCVCModel>();
                }
                var jsonContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<ReferenceReceivedFromCVCModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }

                return new List<ReferenceReceivedFromCVCModel>();
            }
            catch (Exception)
            {
                return new List<ReferenceReceivedFromCVCModel>();
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

                var requestUrl = $"{BaseUrl}OfficerPostingDetails/addOfficerPostingDetails";

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
                {
                    return result;
                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return 0;
            }
        }

        // Added code as on date 02_07_2025

        public async Task<List<OfficerListModel>> GetOfficerListAsync(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<OfficerListModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}OfficerDetails/OfficerDetailsGetByMasterReferenceID?id={Uri.EscapeDataString(id)}";  // Pahle Ye API Use Ho tahi thi
            //var requestUrl = $"{BaseUrl}OfficerDetails/OfficerDetails_GetByOfficerId_ForMinistry?OfficerId={Uri.EscapeDataString(id)}";    

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<OfficerListModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<OfficerListModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }

                return new List<OfficerListModel>();
            }
            catch (Exception)
            {
                return new List<OfficerListModel>();
            }
        }

        // Added code as on date 02_07_2025

        // Added as on date 03_07_2025

        public async Task<List<OfficerPostingDetailsViewModellist>> GetOfficerPostingList(string id)
        {

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<OfficerPostingDetailsViewModellist>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}OfficerPostingDetails/OfficerPostingDetailsGetByOfficerId?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<OfficerPostingDetailsViewModellist>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                //var apiResponse = JsonConvert.DeserializeObject<OfficerListAPIModel<OfficerListModel>>(jsonContent);

                // var apiResponse = JsonConvert.DeserializeObject<OfficerListAPIModel>(jsonContent);
                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<OfficerPostingDetailsViewModellist>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    /*return new List<OfficerListModel>();*//* { apiResponse.data }*//*;*/
                    return apiResponse.data;
                }

                return new List<OfficerPostingDetailsViewModellist>();
            }
            catch (Exception)
            {
                return new List<OfficerPostingDetailsViewModellist>();
            }
        }
        // Added as on date 03_07_2025


        #region Added as on date 03_07_2025

        public async Task<int> InsertIntegrityAgreedOrDoubtful(InsertIntegrityAgreedOrDoubtfulModel _insertIntegritymodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedForInsert";
                var requestUrl = $"{BaseUrl}IntegrityAgreedOrDoubtful/IntegrityAgreedOrDoubtful";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_insertIntegritymodel, options);
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


        public async Task<List<InsertIntegrityAgreedOrDoubtfulModel>> GetInsertIntegrityAgreedOrDoubtfulList(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<InsertIntegrityAgreedOrDoubtfulModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}IntegrityAgreedOrDoubtful/IntegrityAgreedOrDoubtfulGetOfficerID?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<InsertIntegrityAgreedOrDoubtfulModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<InsertIntegrityAgreedOrDoubtfulModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }
                return new List<InsertIntegrityAgreedOrDoubtfulModel>();
            }
            catch (Exception)
            {
                return new List<InsertIntegrityAgreedOrDoubtfulModel>();
            }
        }

        #endregion

        #region 04_07_2025
        public async Task<List<AllegationOfMisconductExaminedModel>> GetAllegationOfMisconductExaminedList(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<AllegationOfMisconductExaminedModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}AllegationOfMisconductExamined_9/AllegationOfMisconductExamined?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<AllegationOfMisconductExaminedModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<AllegationOfMisconductExaminedModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }
                return new List<AllegationOfMisconductExaminedModel>();
            }
            catch (Exception ex)
            {
                return new List<AllegationOfMisconductExaminedModel>();
            }
        }


        public async Task<int> InsertAllegationOfMisconductExamined(AllegationOfMisconductExaminedModel _insertAllegationOfMisconduct)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}AllegationOfMisconductExamined_9/AddAllegationOfMisconductExamined";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_insertAllegationOfMisconduct, options);
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

        //10.
        public async Task<int> InPunishmentAwarded(PunishmentAwardedModel _punishmentAwarded)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}PunishmentAwarded_10/AddPunishmentAwarded";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_punishmentAwarded, options);
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


        public async Task<List<PunishmentAwardedModel>> GetPunishmentAwardedList(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<PunishmentAwardedModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}PunishmentAwarded_10/GetPunishmentAwardedlist?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<PunishmentAwardedModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<PunishmentAwardedModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }
                return new List<PunishmentAwardedModel>();
            }
            catch (Exception ex)
            {
                return new List<PunishmentAwardedModel>();
            }
        }

        #endregion

        //11

        public async Task<int> InsertDisciplinaryCriminalProceedings(DisciplinaryCriminalProceedingsModel _disciplinaryCriminalProceedings)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}DisciplinaryCriminalProceedings_11/addDisciplinaryCriminalProceedings";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_disciplinaryCriminalProceedings, options);
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


        public async Task<List<DisciplinaryCriminalProceedingsModel>> GetDisciplinaryCriminalProceedingsModelList(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<DisciplinaryCriminalProceedingsModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}DisciplinaryCriminalProceedings_11/getdisciplinaryCriminalProceedingslist?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<DisciplinaryCriminalProceedingsModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<DisciplinaryCriminalProceedingsModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }
                return new List<DisciplinaryCriminalProceedingsModel>();
            }
            catch (Exception ex)
            {
                return new List<DisciplinaryCriminalProceedingsModel>();
            }
        }

        //12

        public async Task<int> InsertActionContemplatedAgainstTheOfficer(ActionContemplatedAgainstTheOfficerModel _actionContemplatedAgainstTheOfficerModel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}ActionContemplatedAgainstTheOfficer_12/addActionContemplatedAgainstTheOfficer";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_actionContemplatedAgainstTheOfficerModel, options);
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

        public async Task<List<ActionContemplatedAgainstTheOfficerModel>> GetActionContemplatedAgainstTheOfficerlList(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<ActionContemplatedAgainstTheOfficerModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}ActionContemplatedAgainstTheOfficer_12/getActionContemplatedAgainstTheOfficelist?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<ActionContemplatedAgainstTheOfficerModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<ActionContemplatedAgainstTheOfficerModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }
                return new List<ActionContemplatedAgainstTheOfficerModel>();
            }
            catch (Exception ex)
            {
                return new List<ActionContemplatedAgainstTheOfficerModel>();
            }
        }

        //13

        public async Task<int> InsertComplaintWithVigilanceAnglePending(ComplaintWithVigilanceAnglePendingModel _vigilanceAnglePendingModel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}ComplaintWithVigilanceAnglePending_13/addComplaintWithVigilanceAnglePending";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_vigilanceAnglePendingModel, options);
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


        public async Task<List<ComplaintWithVigilanceAnglePendingModel>> GetComplaintWithVigilanceAnglePendingList(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<ComplaintWithVigilanceAnglePendingModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{BaseUrl}ComplaintWithVigilanceAnglePending_13/getComplaintWithVigilanceAnglePendinglist?id={Uri.EscapeDataString(id)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<ComplaintWithVigilanceAnglePendingModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<ComplaintWithVigilanceAnglePendingModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }
                return new List<ComplaintWithVigilanceAnglePendingModel>();
            }
            catch (Exception ex)
            {
                return new List<ComplaintWithVigilanceAnglePendingModel>();
            }
        }


        // Start the code for the New Reference To CVC
        public async Task<int> Insert_New_Reference(InsertReferenceReceivedForModel _insertRefMode)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}VcReferenceReceivedFor/InsertReferenceReceivedFor";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_insertRefMode, options);
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

        #region Added as on date 14_07_2025

        public async Task<int> Insert_New_Officer_Details(InsertNewOfficerDetailFromMinistryModel insertNewOfficerDetailFromMinistryModel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}OfficerDetails/addOfficerDetails";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(insertNewOfficerDetailFromMinistryModel, options);
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

        #endregion


        #region Added as on date 16_07_2025
        public async Task<int> UpdateReferenceFromPESB(UpdateRefFromPESBModel _updateRefModel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{BaseUrl}VcReferenceReceivedFor/UpdateReferenceFromPESB";

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var json = System.Text.Json.JsonSerializer.Serialize(_updateRefModel, options);
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



        public async Task<List<OfficerListModel>> GetOfficerDetailsByOfficerIdAsync(string id)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<OfficerListModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //var requestUrl = $"{BaseUrl}OfficerDetails/OfficerDetailsGetByMasterReferenceID?id={Uri.EscapeDataString(id)}";  // Pahle Ye API Use Ho tahi thi
            var requestUrl = $"{BaseUrl}OfficerDetails/OfficerDetails_GetByOfficerId_ForMinistry?OfficerId={Uri.EscapeDataString(id)}";    

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<OfficerListModel>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<FetchAPIDataModel<OfficerListModel>>(jsonContent);

                if (apiResponse != null && apiResponse.isSuccess && apiResponse.data != null)
                {
                    return apiResponse.data;
                }

                return new List<OfficerListModel>();
            }
            catch (Exception)
            {
                return new List<OfficerListModel>();
            }
        }


        #endregion



        // End the code for the New Reference To CVC

    }
}
