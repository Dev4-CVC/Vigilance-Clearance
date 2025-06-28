using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblTransaction13ComplaintWithVigilanceAnglePending
{
    public long Id { get; set; }

    public long? OfficerId { get; set; }

    public string? WhetherVigilanceAngelPending { get; set; }

    public string? DetailsOfTheCase { get; set; }

    public string? PresentStatusOftheCase { get; set; }

    public string? AddtionalRemarks { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }
}
