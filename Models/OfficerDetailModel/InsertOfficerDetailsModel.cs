namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class InsertOfficerDetailsModel
    {
        //public bool isSuccess { get; set; }
        //public string message { get; set; }
        //public string messageType { get; set; }
        //public string data { get; set; }
        //public int vcOfficerId { get; set; }
        public string orgCode { get; set; }
        public string designation { get; set; }
        public string placeOfPosting { get; set; }
        public string orgMinistry { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBySessionId { get; set; }
        public string createdByIp { get; set; }
        
    }
}
