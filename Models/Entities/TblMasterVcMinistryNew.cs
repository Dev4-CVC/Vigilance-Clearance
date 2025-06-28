using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblMasterVcMinistryNew
{
    public long Id { get; set; }

    public string? MinName { get; set; }

    public string? OrgName { get; set; }

    public string? BranchOffice { get; set; }

    public string? OrgCode { get; set; }

    public string? MinFileHead { get; set; }

    public string? OrgFileHead { get; set; }

    public int? Section { get; set; }

    public string? Category { get; set; }

    public string? Bocode { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Createdby { get; set; }

    public DateTime? Createdon { get; set; }

    public string? CreatedbyIp { get; set; }

    public string? Updatedby { get; set; }

    public DateTime? Updatedon { get; set; }

    public string? UpdatedbyIp { get; set; }

    public string? SessionId { get; set; }

    public int? PrevId { get; set; }

    public int? Sno { get; set; }

    public string? AsCode { get; set; }

    public string? Remarks { get; set; }

    public string? OrgCodeOld { get; set; }

    public string? Bocode27022024 { get; set; }

    public string? UpdateFlag { get; set; }

    public string? MinCode { get; set; }
}
