namespace VigilanceClearance.Models.Modal_Properties.PESB
{
    public class OfficerPostingDetails_Model
    {
        public int vcOfficerId { get; set; }
        public string orgCode { get; set; }
        public string orgMinistry { get; set; }
        public string designation { get; set; }
        public string placeOfPosting { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }

        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }
        
    }


}
