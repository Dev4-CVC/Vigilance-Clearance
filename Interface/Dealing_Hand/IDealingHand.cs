using VigilanceClearance.Models.Dealing_Hand;
using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Interface.Dealing_Hand
{
    public interface IDealingHand
    {
        Task<List<PendingListFor_BranchDH>> Get_PendingListFor_BranchDH_Async(string branch);
        Task<List<PendingOfficerDetailsForCVCUsers>> Get_Pending_Officer_Details_For_CVC_Users_Async(int id, string branch);
    }
}
