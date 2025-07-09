using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerIntegrityAgreedOrDoubtful
    {
        [Display(Name = "Entered In The List")]
        public string? IsAgreed { get; set; }
        public DateTime YearFrom { get; set; }
        public DateTime YearTo { get; set; }
        public DateTime RemovedFromAgreedlistDate { get; set; }
    }
}
