namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class InsertOfficerDetailsModel
    {
        //public bool isSuccess { get; set; }
        //public string message { get; set; }
        //public string messageType { get; set; }
        //public string data { get; set; }
        //public int VcOfficerId { get; set; }
        //public string orgCode { get; set; }
        //public string designation { get; set; }
        //public string placeOfPosting { get; set; }
        //public string orgMinistry { get; set; }
        //public string fromDate { get; set; }
        //public string toDate { get; set; }
        //public string createdBy { get; set; }
        //public string createdBySessionId { get; set; }
        //public string createdByIp { get; set; }



        public int VcOfficerId { get; set; }

        public string OrgCode { get; set; }

        public string Designation { get; set; }
        public string PlaceOfPosting { get; set; }
        public string OrgMinistry { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedBySessionId { get; set; }
        public string CreatedByIp { get; set; }


    }
}
