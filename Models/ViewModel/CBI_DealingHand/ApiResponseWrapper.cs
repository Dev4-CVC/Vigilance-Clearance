namespace VigilanceClearance.Models.ViewModel.CBI_DealingHand
{
    public class ApiResponseWrapper<T>

    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string messageType { get; set; }
        public T data { get; set; }
        //public List<VcReferenceReceivedForGetById_Model> datalist { get; set; }

    }
}
