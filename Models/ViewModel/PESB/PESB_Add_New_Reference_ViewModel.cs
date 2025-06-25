using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.PESB
{
    public class PESB_Add_New_Reference_ViewModel
    {
        [Required(ErrorMessage = "Please select Post")]
        public string? PostCode { get; set; }
        public string? Postcode { get; set; }

        public string? Description { get; set; }
        public string? PostDescription { get; set; }
        public List<SelectListItem> PostDescriptionList { get; set; } = new();

        [Required(ErrorMessage = "Please select Sub Post")]
        public string? SubPostCode { get; set; }

        public string? SubPostDescription { get; set; }
        public List<SelectListItem> SubPostDescriptionList { get; set; } = new();


        public string? ReferenceCode { get; set; }
        public string? ReferenceDesc { get; set; }
        public List<SelectListItem> ReferenceDescList { get; set; } = new();


        public string? OtherSubPost { get; set; }

    }
}
