namespace VigilanceClearance.Models.ViewModel
{
    public class TestModel
    {
        public int id { get; set; }
        public string? postCode { get; set; }
        public string? postDescription { get; set; }
        public string? createdBy { get; set; }
        public DateTime? createdOn { get; set; }
        public string? createdByIp { get; set; }
        public string? createdBySession { get; set; }
        public string? updateBy { get; set; }
        public DateTime? updatedOn { get; set; }   
        public string? updatedBySessionId { get; set; }
        public string? updatedByIp { get; set; }
    }
}
