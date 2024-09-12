using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Branch
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<BranchCharacteristicsList> BranchCharacteristicsLists { get; set; } = new List<BranchCharacteristicsList>();

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
