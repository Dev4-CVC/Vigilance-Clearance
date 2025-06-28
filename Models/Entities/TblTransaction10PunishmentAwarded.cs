using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblTransaction10PunishmentAwarded
{
    public long Id { get; set; }

    public long? OfficerId { get; set; }

    public string? PunishmentAwarded { get; set; }

    public string? PunishmentDetails { get; set; }

    public DateTime? PunishmentFromDate { get; set; }

    public DateTime? PunishmentToDate { get; set; }

    public DateTime? CheckNameFromDate { get; set; }

    public DateTime? CheckNameToDate { get; set; }

    public string? AdditionalRemarksIfAny { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }
}
