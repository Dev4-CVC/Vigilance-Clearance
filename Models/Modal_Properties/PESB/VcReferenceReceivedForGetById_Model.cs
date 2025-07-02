using VigilanceClearance.Models.ViewModel.PESB;

namespace VigilanceClearance.Models.Modal_Properties.PESB
{
    public class VcReferenceReceivedForGetById_Model
    {
       public List<VcReferenceReceivedFor_ViewModel> data_List { get; set; }

        public int Id { get; set; }
        public string ReferenceReceivedFor { get; set; }
        public string ReferenceReceivedFrom { get; set; }
        public string ReferenceReceivedFromCode { get; set; }
        public string selectionForThePostCategory { get; set; }
        public string selectionForThePostSubCategory { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string MinCode { get; set; }
        public string MinDesc { get; set; }
        public string ReferenceNo_FileNo { get; set; }
        public DateTime? ReferenceOrSubmissionToCvcDate { get; set; }
        public string CVC_ReferenceID_FileNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy_SessionId { get; set; }
        public string CreatedBy_IP { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy_SessionId { get; set; }
        public string UpdatedBy_IP { get; set; }
        public string PendingWith { get; set; }
        public string UID { get; set; }
        public string ReferenceID { get; set; }

    }
}
