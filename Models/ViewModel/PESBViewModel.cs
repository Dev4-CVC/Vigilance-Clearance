using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel
{
    public class PESBViewModel
    {
        [Required(ErrorMessage = "Please select Post")]
        public string? PostCode { get; set; }

        public string? PostDescription { get; set; }
        public List<SelectListItem> PostDescriptionList { get; set; } = new();

        [Required(ErrorMessage = "Please select Sub Post")]
        public string? SubPostCode { get; set; }

        public string? SubPostDescription { get; set; }
        public List<SelectListItem> SubPostDescriptionList { get; set; } = new();

        public string? OtherSubPost { get; set; }



        [Required(ErrorMessage = "Please enter Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter Date of Retirement")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfRetirement { get; set; }

        [Required(ErrorMessage = "Please enter Date of Entry into Service")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfEntry { get; set; }







        [Required(ErrorMessage = "Please select Service")]
        public string? Service { get; set; }
        public List<SelectListItem> ServiceList { get; set; } = new();


        [Required(ErrorMessage = "Please select Batch")]
        public string? Batch { get; set; }
        public List<SelectListItem> BatchList { get; set; } = new();


        [Required(ErrorMessage = "Please select Cadre")]
        public string? Cadre { get; set; }
        public List<SelectListItem> CadreList { get; set; } = new();

        public string? OtherService { get; set; }

    }


}
