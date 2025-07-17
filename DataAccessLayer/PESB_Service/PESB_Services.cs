using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VigilanceClearance.Interface.PESB;
using VigilanceClearance.Models;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel.Ministry;
using VigilanceClearance.Models.ViewModel.PESB;

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
        //Action Method: PESB_Add_New_Reference
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
        //Action Method: PESB_Add_New_Reference 
        //(Select Item List)
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
        //Action Method: PESB_Add_New_Reference 
        //(Select Item List)
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
        //Action Method: PESB_Add_New_Reference
        //(Select Item List)
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
        //Action Method: PESB_Add_New_Reference
        //(Select Item List)
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


        //db table name: tbl_Master_Vc_ReferenceReceivedFor
        //Action Method: PESB_Add_New_Reference  
        //(Insert Add new reference)
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





        //db table name: tbl_Master_Vc_ReferenceReceivedFor
        //Action Method: PESB_Appointment
        //(List of new references)
        public async Task<List<VcReferenceReceivedFor_VM>> Get_VC_ReferenceReceivedFor_List_GetByIdAsync(int id, string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return new List<VcReferenceReceivedFor_VM>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<VcReferenceReceivedFor_VM>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedForGetById?id={id}&UserName={Uri.EscapeDataString(username)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<VcReferenceReceivedFor_VM>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<List<VcReferenceReceivedFor_VM>>>(jsonContent);

                return apiResponse?.isSuccess == true && apiResponse.data != null ? apiResponse.data : new List<VcReferenceReceivedFor_VM>();
            }
            catch (Exception)
            {
                return new List<VcReferenceReceivedFor_VM>();
            }
        }



        //db table name: tbl_Master_Vc_ReferenceReceivedFor
        //Action Method: New_Reference_Details
        //(Details of new reference)
        public async Task<List<VcReferenceReceivedFor_VM>> Get_VC_ReferenceReceivedFor_Details_Async(int id)
        {
            if (id <= 0) return new List<VcReferenceReceivedFor_VM>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<VcReferenceReceivedFor_VM>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedDetailsGetById?id={id}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<VcReferenceReceivedFor_VM>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<List<VcReferenceReceivedFor_VM>>>(jsonContent);

                return apiResponse?.isSuccess == true && apiResponse.data != null ? apiResponse.data : new List<VcReferenceReceivedFor_VM>();
            }
            catch (Exception)
            {
                return new List<VcReferenceReceivedFor_VM>();
            }
        }





        //db table name: 
        //Action Method: PESB Reports  
        public async Task<int> InsertOfficerDetailsAsync(OfficerDetails_Model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}OfficerDetails/addOfficerDetails";

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


        public async Task<int> InsertOfficerPostingDetailsAsync(OfficerPostingDetails_Model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}OfficerPostingDetails/addOfficerPostingDetails";

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










        //=================================================================================
        //=================================================================================
        //new operation:  dated on 9 july 2025
        //new reference 
        public async Task<int> Insert_Add_New_Reference_Async(new_reference_model objmodel)
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
        public async Task<List<new_reference_model>> Get_Vc_Reference_Received_For_List_GetById_and_Username_Async(int id, string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return new List<new_reference_model>();

            var context = _httpContextAccessor.HttpContext;
            if (context == null) return new List<new_reference_model>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<new_reference_model>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedForGetById?id={id}&UserName={Uri.EscapeDataString(username)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<new_reference_model>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<List<new_reference_model>>>(jsonContent);

                return apiResponse?.isSuccess == true && apiResponse.data != null ? apiResponse.data : new List<new_reference_model>();
            }
            catch (Exception)
            {
                return new List<new_reference_model>();
            }
        }
        public async Task<List<new_reference_model>> Get_Vc_Reference_Received_For_Details_GetById_Async(int id)
        {
            if (id <= 0) return new List<new_reference_model>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<new_reference_model>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}VcReferenceReceivedFor/VcReferenceReceivedDetailsGetById?id={id}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<new_reference_model>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<List<new_reference_model>>>(jsonContent);

                return apiResponse?.isSuccess == true && apiResponse.data != null ? apiResponse.data : new List<new_reference_model>();
            }
            catch (Exception)
            {
                return new List<new_reference_model>();
            }
        }


        //officer details and officer posting details
        public async Task<int> Insert_Officer_Details_Async(officer_details_model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}OfficerDetails/addOfficerDetails";

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
        public async Task<int> Insert_Officer_Posting_Details_Async(officer_posting_details_model objmodel)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    throw new UnauthorizedAccessException("Access token is missing.");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var requestUrl = $"{APIURL}OfficerPostingDetails/addOfficerPostingDetails";

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
        public async Task<List<SelectListItem>> Get_Service_DropDownAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}DropDown/ServiceGetAll");

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
        public async Task<List<SelectListItem>> Get_Cadre_DropDownAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<SelectListItem>();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync($"{APIURL}DropDown/CadreGetAll");

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
        public Task<List<SelectListItem>> Get_Batch_DropDownAsync()
        {
            try
            {
                var batchList = new List<SelectListItem>();

                for (int year = 1970; year <= 2070; year++)
                {
                    batchList.Add(new SelectListItem
                    {
                        Value = year.ToString(),
                        Text = year.ToString()
                    });
                }

                return Task.FromResult(batchList);
            }
            catch (Exception)
            {
                return Task.FromResult(new List<SelectListItem>());
            }
        }
        public async Task<List<officer_details_model>> Get_Officer_List_GetById_Async(int id)
        {
            if (id <= 0) return new List<officer_details_model>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<officer_details_model>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}OfficerDetails/OfficerDetailsGetByMasterReferenceID?id={id}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<officer_details_model>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<List<officer_details_model>>>(jsonContent);

                return apiResponse?.isSuccess == true && apiResponse.data != null
                    ? apiResponse.data
                    : new List<officer_details_model>();
            }
            catch (Exception)
            {
                return new List<officer_details_model>();
            }
        }
        public async Task<List<officer_posting_details_model>> Get_Officer_Posting_List_GetById_Async(int id)
        {
            if (id <= 0) return new List<officer_posting_details_model>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<officer_posting_details_model>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}OfficerPostingDetails/OfficerPostingDetailsGetByOfficerId?id={id}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<officer_posting_details_model>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<List<officer_posting_details_model>>>(jsonContent);

                return apiResponse?.isSuccess == true && apiResponse.data != null
                    ? apiResponse.data
                    : new List<officer_posting_details_model>();
            }
            catch (Exception)
            {
                return new List<officer_posting_details_model>();
            }
        }
        public async Task<List<officer_details_model>> Get_Officer_Details_Async(int id)
        {
            if (id <= 0) return new List<officer_details_model>();

            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken)) return new List<officer_details_model>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestUrl = $"{APIURL}OfficerDetails/OfficerDetailsGetById?id={id}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return new List<officer_details_model>();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponseWrapper<officer_details_model>>(jsonContent);

                if (apiResponse?.isSuccess == true && apiResponse.data != null)
                {
                    return new List<officer_details_model> { apiResponse.data };
                }
                else
                {
                    return new List<officer_details_model>();
                }

            }
            catch (Exception)
            {
                return new List<officer_details_model>();
            }
        }
    }
}
