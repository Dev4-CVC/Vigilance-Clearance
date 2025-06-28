using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class MenuItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Url { get; set; }

    public int? ParentId { get; set; }

    public int DisplayOrder { get; set; }

    public bool? IsActive { get; set; }

    public string? RequiredRole { get; set; }
}
