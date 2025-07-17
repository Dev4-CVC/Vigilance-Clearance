using VigilanceClearance.Models.Modal_Properties.Account;

namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class OfficerListAPIModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }        
        //public List<OfficerListModel> data { get; set; }
        public UserDetailsModel data { get; set; }

    }
}
