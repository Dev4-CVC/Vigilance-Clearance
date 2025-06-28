namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class OfficerListModel
    {
        public int Id { get; set; }
        public int MasterReferenceID { get; set; }
        public string selectionForThePostCategory { get; set; }
        public string selectionForThePostSubCategory { get; set; }
        public string ReferenceReceivedFor { get; set; }
        public string othersRemarks { get; set; }
        public string orgCode { get; set; }
        public string Officer_Name { get; set; }
        public string Officer_FatherName { get; set; }
        public DateTime Officer_DateOfBirth { get; set; }
        public DateTime Officer_RetirementDate { get; set; }
        public DateTime Officer_ServiceEntryDate { get; set; }
        public string Officer_Service { get; set; }
        public int Officer_Batch_Year { get; set; }
        public string Officer_Cadre { get; set; }
        public string IntegrityAgreedOrDoubtful_8 { get; set; }
        public string AllegationOfMisconductExamined_9 { get; set; }
        public string PunishmentAwarded_10 { get; set; }
        public string DisciplinaryCriminalProceedings_11 { get; set; }
        public string ActionContemplatedAgainstTheOfficerAsOnDate_12 { get; set; }
        public string ComplaintWithVigilanceAnglePending_13 { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy_SessionId { get; set; }
        public string CreatedBy_IP { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy_SessionId { get; set; }
        public string UpdatedBy_IP { get; set; }
        public object PendingWith { get; set; }
        public string UID { get; set; }

        public OfficerListModel data { get; set; } // not a List
    }
}
