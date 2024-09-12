using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Offer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly? WorkStart { get; set; }

    public DateOnly? WorkEnd { get; set; }

    public DateTime PublishStart { get; set; }

    public DateTime? PublishEnd { get; set; }

    public string Paid { get; set; } = null!;

    public string? NegotiatedSalary { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string RemoteWork { get; set; } = null!;

    public DateTime LastUpdate { get; set; }

    public string PrivateStatus { get; set; } = null!;

    public virtual ICollection<OfferCharacteristicsList> OfferCharacteristicsLists { get; set; } = new List<OfferCharacteristicsList>();

    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
