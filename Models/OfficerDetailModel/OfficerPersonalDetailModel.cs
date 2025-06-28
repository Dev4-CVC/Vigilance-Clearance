using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerPersonalDetailModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name of the Officer")]
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
        //public List<SelectListItem> OrganizationList { get; internal set; }
    }
}
