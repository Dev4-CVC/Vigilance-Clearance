using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerActionContemplatedAgainstTheOfficerAsOnDate
    {
        [Display(Name = "Case Contemplated")]
        public string? whether_CaseContemplated { get; set; }

        [Display(Name = "Case Details")]
        public string? detailsOfTheCase { get; set; }

        [Display(Name = "Present Status Of the Case")]
        public string? presentStatusOftheCase { get; set; }
      
    }
}
