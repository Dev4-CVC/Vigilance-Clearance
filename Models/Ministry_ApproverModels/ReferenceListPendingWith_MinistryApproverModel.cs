using VigilanceClearance.Models.ViewModel.Ministry;

namespace VigilanceClearance.Models.Ministry_ApproverModels
{
    public class ReferenceListPendingWith_MinistryApproverModel
    {
        public int Id { get; set; }
        public string ReferenceReceivedFor { get; set; }
        public string ReferenceReceivedFrom { get; set; }
        public string ReferenceReceivedFromCode { get; set; }
        public string SelectionForThePostCategory { get; set; }
        public string SelectionForThePostSubCategory { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }  // Can be null
        public string MinCode { get; set; }
        public string MinDesc { get; set; }  // Can be null
        public string ReferenceNo_FileNo { get; set; }  // Can be null
        public DateTime? ReferenceOrSubmissionToCvcDate { get; set; }  // Nullable DateTime
        public string CVC_ReferenceID_FileNo { get; set; }
        public string PendingWith { get; set; }
        public Guid UID { get; set; }
        public string ReferenceID { get; set; }

        public ReferenceListPendingWith_MinistryApproverModel data { get; set; } // not a List
    }
}
