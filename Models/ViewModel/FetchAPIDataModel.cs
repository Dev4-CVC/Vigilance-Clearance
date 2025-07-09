using VigilanceClearance.Models.ViewModel.Ministry;

namespace VigilanceClearance.Models.ViewModel
{
    public class FetchAPIDataModel<T>where T : class
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }      
        public List<T> data { get; set; }
    }
}
