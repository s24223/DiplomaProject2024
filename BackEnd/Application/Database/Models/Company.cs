using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Company
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual User User { get; set; } = null!;
}
