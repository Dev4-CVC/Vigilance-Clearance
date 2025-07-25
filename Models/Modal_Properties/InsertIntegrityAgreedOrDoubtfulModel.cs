namespace VigilanceClearance.Models.Modal_Properties
{
    public class InsertIntegrityAgreedOrDoubtfulModel
    {
        public int masterReferenceId { get; set; } 
        public int id { get; set; }
        public int officerId { get; set; }
        public string enteredInTheList { get; set; }
        public DateTime dateOfEntryInTheList { get; set; }
        public string removedFromTheList { get; set; }
        public string dateOfRemovalFromTheList { get; set; }
        public string actionBy { get; set; }
        public DateTime actionOn { get; set; }
        public string actionBy_SessionId { get; set; }
        public string actionBy_IP { get; set; }


    }
}
