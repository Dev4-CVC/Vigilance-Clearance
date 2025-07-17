namespace VigilanceClearance.Models.ViewModel.CBI_DealingHand
{
    public class OfficerDetailWithFeedbackModel
    {
        public int MasterReferenceID { get; set; }
        public int Id { get; set; }
        public string Officer_Name { get; set; }
        public string Officer_FatherName { get; set; }
        public DateTime? Officer_DateOfBirth { get; set; }
        public DateTime? Officer_RetirementDate { get; set; }
        public DateTime? Officer_ServiceEntryDate { get; set; }
        public string Officer_Batch_Year { get; set; }
        
        public string Officer_Cadre { get; set; }
        public int? cbiId { get; set; }
        public int? officerId { get; set; }
        public string FeebbackOf_CBI { get; set; }

       
    }
}
