namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerIntegrityAgreedOrDoubtful
    {
        public bool? IsAgreed { get; set; }
        public DateTime YearFrom { get; set; }
        public DateTime YearTo { get; set; }
        public DateTime RemovedFromAgreedlistDate { get; set; }
    }
}
