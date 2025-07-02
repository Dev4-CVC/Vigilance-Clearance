using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Interface.PESB
{
    public interface IPESB
    {
        Task<List<SelectListItem>> GetReferenceDropDownAsync();
        Task<List<SelectListItem>> GetPostDescriptionsDropDownAsync(string ReferenceCode);
        Task<List<SelectListItem>> GetSubPostsByPostCodeInternal(string postCode);
        Task<List<SelectListItem>> GetOrganizationDropDownAsync(string id);
        Task<List<SelectListItem>> GetMinistryByPostCodeInternal(string id);
        Task<int> InsertAddNewReferenceAsync(PESB_Add_New_Reference_Model objmodel);



        //Action Method: PESB Appointment
        Task<List<VcReferenceReceivedFor_ViewModel>> Get_VC_ReferenceReceivedFor_List_GetByIdAsync(int id, string username);
    }
}