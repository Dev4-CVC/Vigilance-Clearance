using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.Modal_Properties.PESB;
using VigilanceClearance.Models.OfficerDetailModel;

namespace VigilanceClearance.Interface.Ministry
{
    public interface IMinistry
    {
        Task<List<SelectListItem>> GetOrganizationDropDownAsync(string section);
        Task<List<SelectListItem>> GetMinistryDropDownbycodeAsync(string orgcode); 
        Task<int> InsertOfficerPostingDetail(InsertOfficerDetailsModel objmodel);

        //Task<IActionResult> ViewOfficerListForGivenProposal(string id);
    }
}
 