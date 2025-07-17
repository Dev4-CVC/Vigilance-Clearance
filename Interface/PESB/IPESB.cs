using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.PESB;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Interface.PESB
{
    public interface IPESB
    {
        //Action Method: PESB_Add_New_Reference
        Task<List<SelectListItem>> GetReferenceDropDownAsync();
        Task<List<SelectListItem>> GetPostDescriptionsDropDownAsync(string ReferenceCode);
        Task<List<SelectListItem>> GetSubPostsByPostCodeInternal(string postCode);
        Task<List<SelectListItem>> GetOrganizationDropDownAsync(string id);
        Task<List<SelectListItem>> GetMinistryByPostCodeInternal(string id);
        Task<int> InsertAddNewReferenceAsync(PESB_Add_New_Reference_Model objmodel);



        //Action Method: PESB_Appointment
        Task<List<VcReferenceReceivedFor_VM>> Get_VC_ReferenceReceivedFor_List_GetByIdAsync(int id, string username);


        
        //Action Method: New_Reference_Details
        Task<List<VcReferenceReceivedFor_VM>> Get_VC_ReferenceReceivedFor_Details_Async(int id);



        //Action Method:  PESB Reports
        Task<List<SelectListItem>> Get_Service_DropDownAsync();
        Task<List<SelectListItem>> Get_Batch_DropDownAsync();
        Task<List<SelectListItem>> Get_Cadre_DropDownAsync();
        Task<int> InsertOfficerDetailsAsync(OfficerDetails_Model objmodel);
        Task<int> InsertOfficerPostingDetailsAsync(OfficerPostingDetails_Model objmodel);






        //new operation:  dated on 9 july 2025
        //new reference
        Task<int> Insert_Add_New_Reference_Async(new_reference_model objmodel);
        Task<List<new_reference_model>> Get_Vc_Reference_Received_For_List_GetById_and_Username_Async(int id, string username);
        Task<List<new_reference_model>> Get_Vc_Reference_Received_For_Details_GetById_Async(int id);

        //officer details
        Task<int> Insert_Officer_Details_Async(officer_details_model objmodel);

        //officer posting details
        Task<int> Insert_Officer_Posting_Details_Async(officer_posting_details_model objmodel);


        #region Added as on date 11_07_11

        Task<List<SelectListItem>> GetOrgByMinCode(string Mincode);

        #endregion
    }
}