using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerDisciplinaryCriminalProceedings
    {
        [Display(Name = "Disciplinary Proceeding")]
        public string? DisciplinaryProceeding { get; set; }

        [Display(Name = "Whether Suspended")]
        public string? whether_Suspended { get; set; }

        [Display(Name = "Suspension Date")]
        public DateTime? suspensionDate { get; set; }

        [Display(Name = "Whether Revoked")]
        public string? whetherRevoked { get; set; }

        [Display(Name = "Revocation Date")]
        public DateTime? revocationDate { get; set; }

        [Display(Name = "Case Details")]
        public string? detailsOf_Case { get; set; }

        [Display(Name = "Present Status Of the Case")]
        public string? presentStatusOftheCase { get; set; }
    }
}
