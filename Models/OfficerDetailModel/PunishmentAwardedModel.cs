namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class PunishmentAwardedModel
    {
        public int masterReferenceId { get; set; }
        public int officerId { get; set; }
        public string punishmentAwarded { get; set; }
        public string punishmentDetails { get; set; }
        public DateTime punishmentFromDate { get; set; }
        public DateTime punishmentToDate { get; set; }
        public DateTime checkName_FromDate { get; set; }
        public DateTime checkName_ToDate { get; set; }
        public string additionalRemarks_IfAny { get; set; }
        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }
    }
}
