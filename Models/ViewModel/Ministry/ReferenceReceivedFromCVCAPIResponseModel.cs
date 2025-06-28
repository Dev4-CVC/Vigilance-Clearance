using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VigilanceClearance.Models.ViewModel.Ministry
{
    public class ReferenceReceivedFromCVCAPIResponseModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        //public System.LocalDataStoreSlot data { get; set; }
        //public List<List<List<object>>> data { get; set; }
        //public List<ReferenceReceivedFromCVCModel> data { get; set; } // if it's an array
        public ReferenceReceivedFromCVCModel data { get; set; } // not a List

        //public OfficerListModel data1 { get; set; } // not a List

    }
}
