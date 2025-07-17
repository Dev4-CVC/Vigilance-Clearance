using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerAllegationOfMisconductExamined
    {
        [Display(Name = "Vigilance Angle Examined")]
        public string? vigilanceAngleExamined { get; set; }
        public string caseDetails { get; set; }
        public string presentStatusOfTheCase { get; set; }
        public string actionrecommendedOptions { get; set; }
        public string actionRecommendedDetails { get; set; }
    }
}
