namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class OfficerListAPIModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }        
        public List<OfficerListModel> data { get; set; }
              
    }
}
