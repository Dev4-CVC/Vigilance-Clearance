using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblMasterVcServiceNew
{
    public long ServiceCodeId { get; set; }

    public string? ServiceName { get; set; }

    public string ServiceCode { get; set; } = null!;

    public string? Createdby { get; set; }

    public DateTime? Createdon { get; set; }

    public string? CreatedbyIp { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Updatedon { get; set; }

    public string? UpdatedbyIp { get; set; }

    public string? SessionId { get; set; }

    public bool? IsActive { get; set; }

    public string? ActiveRemark { get; set; }
}
