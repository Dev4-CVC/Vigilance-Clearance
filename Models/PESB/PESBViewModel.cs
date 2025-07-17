using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Models.PESB
{


    public class PESBViewModel
    {
        //insert reference received data
        public new_reference_model new_reference { get; set; }
        public List<new_reference_model> new_reference_List { get; set; }
        public List<SelectListItem> reference_received_for_ddl_List { get; set; }
        public List<SelectListItem>? post_ddl_List { get; set; } = new();        
        public List<SelectListItem> sub_post_ddl_List { get; set; }
        public List<SelectListItem> organization_ddl_List { get; set; }
        public List<SelectListItem> ministry_ddl_List { get; set; }

        //insert Officer details:
        public officer_details_model officer_details { get; set; }
        public List<officer_details_model> officer_details_List { get; set; }
        public List<SelectListItem> service_ddl_List { get; set; }
        public List<SelectListItem> cadre_ddl_List { get; set; }
        public List<SelectListItem> batch_ddl_List { get; set; }


        //insert Officer posting details:
        public officer_posting_details_model officer_posting_details { get; set; }
        public List<officer_posting_details_model> officer_posting_details_List { get; set; }
    }
}
