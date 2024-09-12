using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AdministrativeDivision> AdministrativeDivisions { get; set; } = new List<AdministrativeDivision>();
}
