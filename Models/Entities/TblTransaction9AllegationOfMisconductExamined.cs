using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblTransaction9AllegationOfMisconductExamined
{
    public long Id { get; set; }

    public long? OfficerId { get; set; }

    public string? VigilanceAngleExamined { get; set; }

    public string? CaseDetails { get; set; }

    public string? PresentStatusOfTheCase { get; set; }

    public string? ActionrecommendedOptions { get; set; }

    public string? ActionRecommendedDetails { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }
}
