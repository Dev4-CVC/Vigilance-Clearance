namespace VigilanceClearance.Models.Dealing_Hand
{
    public class PendingOfficerDetailsForCVCUsers
    {
        public long  id { get; set; }
        public string Officer_Name { get; set; }
        public string Officer_FatherName { get; set; }
        public string Officer_Service { get; set; }
        public string Officer_Other_Service { get; set; }
        public string Officer_Cadre { get; set; }
        public string Officer_Batch_Year { get; set; }
        public DateTime? Officer_ServiceEntryDate { get; set; }
        public DateTime? Officer_RetirementDate { get; set; }
    }
}
