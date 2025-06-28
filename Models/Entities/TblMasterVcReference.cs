using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblMasterVcReference
{
    public int Id { get; set; }

    public string? ReferenceDesc { get; set; }

    public string? ReferenceCode { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }
}
