namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class ComplaintWithVigilanceAnglePendingModel
    {
        public int masterReferenceId { get; set; }
        public int officerId { get; set; }
        public string whether_vigilanceAngelPending { get; set; }
        public string detailsOfTheCase { get; set; }
        public string presentStatusOftheCase { get; set; }
        public string addtionalRemarks { get; set; }
        public string actionBy { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }
    }
}
