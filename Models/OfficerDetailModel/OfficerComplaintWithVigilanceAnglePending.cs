using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerComplaintWithVigilanceAnglePending
    {
        [Display(Name = "Vigilance Angel Pending")]
        public string? whether_vigilanceAngelPending { get; set; }
        public string? detailsOfTheCase { get; set; }
        public string? presentStatusOftheCase { get; set; }
        public string? addtionalRemarks { get; set; }
    }
}
