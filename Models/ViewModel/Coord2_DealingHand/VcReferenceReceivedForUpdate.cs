namespace VigilanceClearance.Models.ViewModel.Coord2_DealingHand
{
    public class VcReferenceReceivedForUpdate
    {
        public int id { get; set; }
        public long masterReferenceId { get; set; }
        public long officerId { get; set; }
        public string? PendingWith { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdatedBy_SessionId { get; set; }
        public string? UpdatedBy_IP { get; set; }
    }
}
