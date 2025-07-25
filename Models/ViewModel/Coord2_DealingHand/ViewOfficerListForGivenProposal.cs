using VigilanceClearance.Models.ViewModel.CBI_DealingHand;
using VigilanceClearance.Models.ViewModel.Coord2_DealingHand;

namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class ViewOfficerListForGivenProposal
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        
        public List<OfficerDetailsWithPostings> data { get; set; } = new();
    }
}
