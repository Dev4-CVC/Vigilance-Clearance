using Microsoft.AspNetCore.Mvc.Rendering;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerPostingDetails
    {
        public string? Organization { get; set; }
        public List<SelectListItem> OrganizationList { get; set; } = new();
        public string? Ministry { get; set; }
        public List<SelectListItem> MinistryList { get; set; } = new();
        public string? Designation { get; set; }
        public DateTime TenureFrom { get; set; }
        public DateTime TenureTo { get; set; }

    }
}
