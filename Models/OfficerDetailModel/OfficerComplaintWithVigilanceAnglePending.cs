using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerComplaintWithVigilanceAnglePending
    {
        [Display(Name = "Vigilance Angel Pending")]
        public string? whether_vigilanceAngelPending { get; set; }

        [Display(Name = "Case Details")]
        public string? detailsOfTheCase { get; set; }

        [Display(Name = "Present Status Of the Case")]
        public string? presentStatusOftheCase { get; set; }

        [Display(Name = "Addtional Remarks")]
        public string? addtionalRemarks { get; set; }
    }
}
