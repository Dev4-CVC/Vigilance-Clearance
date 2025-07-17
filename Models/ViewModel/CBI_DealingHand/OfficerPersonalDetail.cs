using VigilanceClearance.Models.OfficerDetailModel;

namespace VigilanceClearance.Models.ViewModel.CBI_DealingHand
{
    public class OfficerPersonalDetail
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        //public System.LocalDataStoreSlot data { get; set; }
        //public List<List<List<object>>> data { get; set; }
        //public List<ReferenceReceivedFromCVCModel> data { get; set; } // if it's an array
        //public OfficerListModel data { get; set; } // not a List

        public List<OfficerPersonalDetailModel> data { get; set; }
    }
}
