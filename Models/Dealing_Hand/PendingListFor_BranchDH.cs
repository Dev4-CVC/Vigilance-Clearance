using VigilanceClearance.Interface.Ministry;

namespace VigilanceClearance.Models.Dealing_Hand
{
    public class PendingListFor_BranchDH
    {
        public long ReferenceId { get; set; }
        public string ReferenceReceivedFor { get; set; }
        public string ReferenceReceivedFrom { get; set; }
        public string selectionForThePostCategory { get; set; }
        public string selectionForThePostSubCategory { get; set; }
        public string MinName { get; set; }
        public string OrgName { get; set; }
        public string MinCode { get; set; }
        public string OrgCode { get; set; }
        public string PendingWith { get; set; }
    }
}
