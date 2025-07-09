namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerPostingDetailsViewModellist
    {
        public int id { get; set; }
        public int Vc_Officer_Id { get; set; }
        public string orgCode { get; set; }
        public string designation { get; set; }
        public string placeOfPosting { get; set; }
        public string orgMinistry { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy_SessionId { get; set; }
        public string CreatedBy_IP { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy_SessionId { get; set; }
        public string UpdatedBy_IP { get; set; }
    }
}
