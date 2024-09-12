using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Recruitment
{
    public Guid OfferId { get; set; }

    public Guid PersonId { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string AcceptedRejected { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual ICollection<Internship> Internships { get; set; } = new List<Internship>();

    public virtual Offer Offer { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
