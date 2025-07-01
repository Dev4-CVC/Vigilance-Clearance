namespace VigilanceClearance.Models.OfficerDetailModel
{
    public class OfficerDisciplinaryCriminalProceedings
    {
        public string? DisciplinaryProceeding { get; set; }
        public string? SuspensionDate { get; set; }
        public string? WhetherRevoked { get; set; }
        public DateTime DateofRevocation { get; set; }
    }
}
