using Microsoft.AspNetCore.Mvc.Rendering;
using VigilanceClearance.Models.OfficerDetailModel;
using VigilanceClearance.Models.ViewModel.Ministry;

namespace VigilanceClearance.Models.New_Reference_to_CVCModels
{
    public class AddNewOfficerMainModel
    {
        public List<SelectListItem> service_ddl_List { get; set; }
        public List<SelectListItem> cadre_ddl_List { get; set; }
        public List<SelectListItem> batch_ddl_List { get; set; }

        public InsertNewOfficerDetailFromMinistryModel insertNewOfficerDetailFromMinistryModel { get; set; }

        public List<OfficerListModel> officerList { get; set; }
        public PESB.new_reference_model new_Reference_Model { get; set; } 
            

    }
}
