using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerActionContemplatedAgainstTheOfficerAsOnDate
    {
        [Display(Name = "Case Contemplated")]
        public string? whether_CaseContemplated { get; set; }
        public string? detailsOfTheCase { get; set; }
        public string? presentStatusOftheCase { get; set; }
        //public DateTime ContemplatedAgainstStatusasondate { get; set; }
    }
}
