using VigilanceClearance.Models.ViewModel.CBI_DealingHand;
using VigilanceClearance.Models.ViewModel.Comments_Model;

namespace VigilanceClearance.Models.ViewModel.Coord2_DealingHand
{
    public class OfficerAndPostingViewModel
    {
        public int MasterReferenceID { get; set; }
        public int Id { get; set; }
        public string Officer_Name { get; set; }
        public string Officer_FatherName { get; set; }
        public DateTime? Officer_DateOfBirth { get; set; }
        public DateTime? Officer_RetirementDate { get; set; }
        public DateTime? Officer_ServiceEntryDate { get; set; }
        public string Officer_Batch_Year { get; set; }

        public string Officer_Cadre { get; set; }
        public List<CBIFeedbackModel> cBIFeedbackModels { get; set; }
        public List<vIGFeedbackModels> vIGFeedbackModels { get; set; }
        public List<mINFeedbackModels> mINFeedbackModels { get; set; }
        public List<DHFeedbackModels> CVC_COORD2_DH_COMMENTS { get; set; }
        public List<SOFeedbackModels> CVC_COORD2_SO_COMMENTS { get; set; }
        public List<BOFeedbackModels> CVC_COORD2_BO_COMMENTS { get; set; }
        public List<PostingDetailsOfOfficer> postingDetailsOfOfficers { get; set; }
       
    }
}
