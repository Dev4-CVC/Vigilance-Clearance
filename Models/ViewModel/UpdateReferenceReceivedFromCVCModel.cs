using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel
{
    public class UpdateReferenceReceivedFromCVCModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Name of the Officer")]
        public string? Officer_Name { get; set; }
        [Display(Name = "Father's Name")]
        public string? Officer_FatherName { get; set; }
        [Display(Name = "Date of Birth")]
        public string? Officer_DateOfBirth { get; set; }
        [Display(Name = "Date Of Retirement")]
        public string? Officer_RetirementDate { get; set; }
        [Display(Name = "Date of Entry into Service")]
        public string? Officer_ServiceEntryDate { get; set; }
        [Display(Name = "Service")]
        public string? Officer_Service { get; set; }
        [Display(Name = "Batch")]
        public string? Officer_Batch_Year { get; set; }

        public string? Cadre { get; set; }


        public string? SelectedCountry { get; set; }

        public List<SelectListItem>? CountryList { get; set; }

//7
        public string? Organization { get; set; }
        public string? Designation { get; set; }
        public DateTime TenureFrom { get; set; }
        public DateTime TenureTo { get; set; }

//8

        public bool? IsAgreed { get; set; }
        public DateTime YearFrom { get; set; }
        public DateTime YearTo { get; set; }

    }
}
