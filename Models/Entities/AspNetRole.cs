﻿using System;
using System.Collections.Generic;

namespace VigilanceClearance.Models.Entities;

public partial class AspNetRole
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
