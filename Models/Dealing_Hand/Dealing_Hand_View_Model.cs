using VigilanceClearance.Models.PESB;

namespace VigilanceClearance.Models.Dealing_Hand
{
    public class Dealing_Hand_View_Model
    {
        public List<PendingListFor_BranchDH> pending_list_for_branch_DH { get; set; }
        public List<PendingListFor_BranchDH> pending_list_for_branch_DH_list { get; set; }

        public List<PendingOfficerDetailsForCVCUsers> pending_officer_details_for_cvc_users { get; set; }
        public List<PendingOfficerDetailsForCVCUsers> pending_officer_details_for_cvc_users_list { get; set; }
    }
}
