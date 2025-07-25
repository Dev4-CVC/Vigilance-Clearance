namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class AllegationOfMisconductExaminedModel
    {
        public int id { get; set; }
        public int masterReferenceId { get; set; }
        public int officerId { get; set; }
        public string vigilanceAngleExamined { get; set; }
        public string caseDetails { get; set; }
        public string presentStatusOfTheCase { get; set; }
        public string actionrecommendedOptions { get; set; }
        public string actionRecommendedDetails { get; set; }
        public string actionBy { get; set; }
        public DateTime actionOn { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }
    }
}
