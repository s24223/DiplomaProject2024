using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Offer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string? IsNegotiatedSalary { get; set; }

    public string IsForStudents { get; set; } = null!;

    public virtual ICollection<BranchOffer> BranchOffers { get; set; } = new List<BranchOffer>();

    public virtual ICollection<OfferCharacteristicsList> OfferCharacteristicsLists { get; set; } = new List<OfferCharacteristicsList>();
}
