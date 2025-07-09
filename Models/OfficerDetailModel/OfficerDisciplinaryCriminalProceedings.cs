using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerDisciplinaryCriminalProceedings
    {
        [Display(Name = "Disciplinary Proceeding")]
        public string? DisciplinaryProceeding { get; set; }
        public string? whether_Suspended { get; set; }
        public DateTime? suspensionDate { get; set; }
        public string? whetherRevoked { get; set; }
        public DateTime? revocationDate { get; set; }
        public string? detailsOf_Case { get; set; }
        public string? presentStatusOftheCase { get; set; }
    }
}
