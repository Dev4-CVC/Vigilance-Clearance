using System;
using System.Collections.Generic;

namespace VigilanceClearance.Models.Entities;

public partial class MasterVcCadre
{
    public int Id { get; set; }

    public string? CadreDesc { get; set; }

    public string? CadreType { get; set; }

    public string? CadreStateUt { get; set; }
}
