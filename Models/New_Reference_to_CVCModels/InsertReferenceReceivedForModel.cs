namespace VigilanceClearance.Models.New_Reference_to_CVCModels
{
    public class InsertReferenceReceivedForModel
    {
        public string referenceReceivedFor { get; set; }
        public string referenceReceivedFrom { get; set; }
        public string referenceReceivedFromCode { get; set; }
        public string selectionForThePostCategory { get; set; }
        public string selectionForThePostSubCategory { get; set; }
        public string orgCode { get; set; }
        public string orgName { get; set; }
        public string minCode { get; set; }
        public string minDesc { get; set; }
        public string pendingWith { get; set; }
        public string referenceID { get; set; }
        public string cvC_ReferenceID_FileNo { get; set; }
        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }
    }
}
