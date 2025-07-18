﻿using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerPunishmentAwarded
    {
        [Display(Name = "Punishment Awarded")]
        public string? punishmentAwarded { get; set; }
        public string punishmentDetails { get; set; }
        public DateTime punishmentFromDate { get; set; }
        public DateTime punishmentToDate { get; set; }
        public DateTime checkName_FromDate { get; set; }
        public DateTime checkName_ToDate { get; set; }
        public string additionalRemarks_IfAny { get; set; }
    }
}
