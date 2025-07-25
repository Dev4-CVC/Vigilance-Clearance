using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerPostingDetails
    {
        [Required(ErrorMessage = "Organization is required.")]
        public string? Organization { get; set; }
        public List<SelectListItem> OrganizationList { get; set; } = new();

        [Required(ErrorMessage = "Ministry is required.")]
        public string? Ministry { get; set; }
        public List<SelectListItem> MinistryList { get; set; } = new();

        [Required(ErrorMessage = "Designation is required.")]
        public string? Designation { get; set; }

        [Display(Name = "Place Of Posting")]
        public string? PlaceOfPosting { get; set; }

        [Required(ErrorMessage = "Tenure From date is required.")]
        public DateTime? TenureFrom { get; set; }

        [Required(ErrorMessage = "Tenure To date is required.")]
        public DateTime? TenureTo { get; set; }

        //public List<OfficerPostingDetails> officerPostingDetails { get; set; }
    }
}
