﻿using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblTransaction8IntegrityAgreedOrDoubtful
{
    public long Id { get; set; }

    public long? OfficerId { get; set; }

    public string? EnteredInTheList { get; set; }

    public DateTime? DateOfEntryInTheList { get; set; }

    public string? RemovedFromTheList { get; set; }

    public string? DateOfRemovalFromTheList { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }
}
