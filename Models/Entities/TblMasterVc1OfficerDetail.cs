using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblMasterVc1OfficerDetail
{
    public long Id { get; set; }

    public string? SelectionForThePostCategory { get; set; }

    public string? SelectionForThePostSubCategory { get; set; }

    public string? ReferenceReceivedFor { get; set; }

    public string? OthersRemarks { get; set; }

    public string? OrgCode { get; set; }

    public string? OfficerName { get; set; }

    public string? OfficerFatherName { get; set; }

    public DateTime? OfficerDateOfBirth { get; set; }

    public DateTime? OfficerRetirementDate { get; set; }

    public DateTime? OfficerServiceEntryDate { get; set; }

    public string? OfficerService { get; set; }

    public int? OfficerBatchYear { get; set; }

    public string? OfficerCadre { get; set; }

    public string? IntegrityAgreedOrDoubtful8 { get; set; }

    public string? AllegationOfMisconductExamined9 { get; set; }

    public string? PunishmentAwarded10 { get; set; }

    public string? DisciplinaryCriminalProceedings11 { get; set; }

    public string? ActionContemplatedAgainstTheOfficerAsOnDate12 { get; set; }

    public string? ComplaintWithVigilanceAnglePending13 { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBySessionId { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBySessionId { get; set; }

    public string? UpdatedByIp { get; set; }

    public string? PendingWith { get; set; }
}
