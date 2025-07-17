namespace VigilanceClearance.Models.ViewModel.Coord2_DealingHand
{
    public class OfficerDetailsWithPostings
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
        public int? id { get; set; }
        public int? Vc_Officer_Id { get; set; }
        public string orgCode { get; set; }
        public string designation { get; set; }
        public string placeOfPosting { get; set; }
        public string orgMinistry { get; set; }
    }
}
