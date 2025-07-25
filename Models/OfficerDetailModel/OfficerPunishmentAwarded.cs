using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerPunishmentAwarded
    {
        [Display(Name = "Punishment Awarded")]
        public string? punishmentAwarded { get; set; }

        [Display(Name = "Punishment Details")]
        public string punishmentDetails { get; set; }

        [Display(Name = "Punishment From_Date")]
        public DateTime punishmentFromDate { get; set; }

        [Display(Name = "Punishment To_Date")]
        public DateTime punishmentToDate { get; set; }

        [Display(Name = "Check Name From_Date")]
        public DateTime checkName_FromDate { get; set; }

        [Display(Name = "Check Name To_Date")]
        public DateTime checkName_ToDate { get; set; }

        [Display(Name = "Additional Remarks_IfAny")]
        public string additionalRemarks_IfAny { get; set; }
    }
}
