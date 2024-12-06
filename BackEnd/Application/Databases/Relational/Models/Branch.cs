using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class Branch
{
    public Guid CompanyId { get; set; }

    public Guid? AddressId { get; set; }

    public Guid Id { get; set; }

    public string? UrlSegment { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<BranchOffer> BranchOffers { get; set; } = new List<BranchOffer>();

    public virtual Company Company { get; set; } = null!;
}
