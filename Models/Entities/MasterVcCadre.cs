using System;
using System.Collections.Generic;

namespace VigilanceClearance.Infrastructure.Infrastructure.Persistence.Models;

public partial class MasterVcCadre
{
    public int Id { get; set; }

    public string? CadreDesc { get; set; }

    public string? CadreType { get; set; }

    public string? CadreStateUt { get; set; }
}
