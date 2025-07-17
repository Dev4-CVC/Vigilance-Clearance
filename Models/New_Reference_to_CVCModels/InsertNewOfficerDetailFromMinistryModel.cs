namespace VigilanceClearance.Models.New_Reference_to_CVCModels
{
    public class InsertNewOfficerDetailFromMinistryModel
    {
        public int? masterReferenceID { get; set; }
        public string officer_Name { get; set; }
        public string officer_FatherName { get; set; }
        public DateTime officer_DateOfBirth { get; set; }
        public DateTime officer_RetirementDate { get; set; }
        public DateTime officer_ServiceEntryDate { get; set; }
        public string officer_Service { get; set; }
        public string? officer_Other_Service { get; set; }
        public int officer_Batch_Year { get; set; }
        public string? officer_Cadre { get; set; }
        public string createdBy { get; set; }
        public string createdBy_SessionId { get; set; }
        public string createdBy_IP { get; set; }
    }
}
