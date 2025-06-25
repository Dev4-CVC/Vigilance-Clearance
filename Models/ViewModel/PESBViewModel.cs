using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel
{
    public class PESBViewModel
    {
        public int Id { get; set; }


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


        [Required(ErrorMessage = "Name of the Applicant is required")]
        public string? ApplicantName { get; set; }

        [Required(ErrorMessage = "Name of Father is required")]
        public string? FatherName { get; set; }




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

        [Required(ErrorMessage = "Please Enter Other Service")]
        public string? OtherService { get; set; }

    }


}
