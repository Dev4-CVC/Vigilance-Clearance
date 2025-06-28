using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class TblMasterVcUser
{
    public int UserId { get; set; }

    public string? UserLogin { get; set; }

    public string? UserName { get; set; }

    public string? UserDesignation { get; set; }

    public string? UserEmail { get; set; }

    public string? UserMobile { get; set; }

    public string? UserBranchOffices { get; set; }

    public string? UserProfile { get; set; }

    public string? UserActiveStatus { get; set; }

    public string? UserPassword { get; set; }

    public int? Section { get; set; }

    public string? MappedSection { get; set; }

    public long? GeneratedOtp { get; set; }

    public long? FilledOtp { get; set; }

    public DateTime? OtpgenerateTime { get; set; }

    public int? FailureCount { get; set; }

    public int? TryCount { get; set; }

    public string? UserPasswordHashed { get; set; }

    public int? ChangePasswordFlag { get; set; }

    public bool? UserLocked { get; set; }

    public string? OrgCode { get; set; }
}
