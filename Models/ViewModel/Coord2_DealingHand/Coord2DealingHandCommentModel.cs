using System.ComponentModel.DataAnnotations;

namespace VigilanceClearance.Models.ViewModel.Coord2_DealingHand
{
    public class Coord2DealingHandCommentModel
    {
        public long? OfficerId { get; set; }
        public long? MasterReferenceId { get; set; }

        public string CreatedBy_Role { get; set; }
        public string CreatedById { get; set; }
        public Coord2DHComments Coord2_DH_comments { get; set; }
    }
}

public class Coord2DHComments
{
    [Required]
    public string AdverseOrNothingAdverse { get; set; }

    public string AdverseRemarks { get; set; }
}