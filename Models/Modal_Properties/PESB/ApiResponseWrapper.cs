using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Models.Modal_Properties.PESB
{
    public class ApiResponseWrapper<T>
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string messageType { get; set; }
        public T data { get; set; } 
        public List<VcReferenceReceivedFor_VM> datalist { get; set; }
    }
}
