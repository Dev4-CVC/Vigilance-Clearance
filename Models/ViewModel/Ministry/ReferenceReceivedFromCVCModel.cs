namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class ReferenceReceivedFromCVCModel
    {
        public int id { get; set; }
        public string ReferenceReceivedFor { get; set; }
        public string ReferenceReceivedFrom { get; set; }
        public string ReferenceReceivedFromCode { get; set; }
        public string selectionForThePostCategory { get; set; }
        public string selectionForThePostSubCategory { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string MinCode { get; set; }
        public string MinDesc { get; set; }
        public object ReferenceNo_FileNo { get; set; }
        public object ReferenceOrSubmissionToCvcDate { get; set; }
        public object CVC_ReferenceID_FileNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy_SessionId { get; set; }
        public string CreatedBy_IP { get; set; }
        public object UpdateBy { get; set; }
        public object UpdatedOn { get; set; }
        public object UpdatedBy_SessionId { get; set; }
        public object UpdatedBy_IP { get; set; }
        public string PendingWith { get; set; }
        public string UID { get; set; }
        public string ReferenceID { get; set; }

        public ReferenceReceivedFromCVCModel data { get; set; }
    }
}
