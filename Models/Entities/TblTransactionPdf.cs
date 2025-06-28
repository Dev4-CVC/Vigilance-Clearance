using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblTransactionPdf
{
    public long Id { get; set; }

    public long? OfficerId { get; set; }

    public string? DocumentId { get; set; }

    public string? DocumentCode { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }
}
