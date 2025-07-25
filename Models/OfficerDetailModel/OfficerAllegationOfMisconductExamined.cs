using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerAllegationOfMisconductExamined
    {
        [Display(Name = "Vigilance Angle Examined")]
        public string? vigilanceAngleExamined { get; set; }

        [Display(Name = "Case Details")]
        public string caseDetails { get; set; }
        [Display(Name = "Present Status Of The Case")]
        public string presentStatusOfTheCase { get; set; }
        [Display(Name = "Action Recommended Options ")]
        public string actionrecommendedOptions { get; set; }
        [Display(Name = "Action Recommended Details")]
        public string actionRecommendedDetails { get; set; }
    }
}
