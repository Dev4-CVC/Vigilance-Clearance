using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblMasterVcMinistry
{
    public int Id { get; set; }

    public string? MinName { get; set; }

    public string? MinCode { get; set; }
}
