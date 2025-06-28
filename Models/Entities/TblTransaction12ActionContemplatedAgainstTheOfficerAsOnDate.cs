using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblTransaction12ActionContemplatedAgainstTheOfficerAsOnDate
{
    public long Id { get; set; }

    public long? OfficerId { get; set; }

    public string? WhetherCaseContemplated { get; set; }

    public string? DetailsOfTheCase { get; set; }

    public string? PresentStatusOftheCase { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }
}
