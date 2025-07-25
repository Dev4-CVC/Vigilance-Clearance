using VigilanceClearance.Models.ViewModel.CBI_DealingHand;

namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class OfficerPersonalDetailModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        
        public List<OfficerDetailWithFeedbackModel> data { get; set; } = new();
    }
}
