namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class DisciplinaryCriminalProceedingsModel
    {
        public int officerId { get; set; }
        public string whether_DisciplinaryCriminalProceedingsPending { get; set; }
        public string whether_Suspended { get; set; }
        public DateTime? suspensionDate { get; set; }
        public string whetherRevoked { get; set; }
        public DateTime? revocationDate { get; set; }
        public string detailsOf_Case { get; set; }
        public string presentStatusOftheCase { get; set; }
        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }
    }
}
