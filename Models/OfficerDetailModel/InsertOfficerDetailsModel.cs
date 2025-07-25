namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class InsertOfficerDetailsModel
    {
        public int masterReferenceId { get; set; }
        public int VcOfficerId { get; set; }

        public string OrgCode { get; set; }

        public string Designation { get; set; }
        public string PlaceOfPosting { get; set; }
        public string OrgMinistry { get; set; }
        //public string FromDate { get; set; }
        //public string ToDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }

    }
}
