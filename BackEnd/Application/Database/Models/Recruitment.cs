using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Recruitment
{
    public Guid PersonId { get; set; }

    public Guid BranchId { get; set; }

    public Guid OfferId { get; set; }

    public DateTime Created { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string? PersonMessage { get; set; }

    public string? CompanyResponse { get; set; }

    public string? IsAccepted { get; set; }

    public byte[]? Cv { get; set; }

    public virtual BranchOffer BranchOffer { get; set; } = null!;

    public virtual ICollection<Internship> Internships { get; set; } = new List<Internship>();

    public virtual Person Person { get; set; } = null!;
}
