using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Internship
{
    public Guid Id { get; set; }

    public Guid OfferId { get; set; }

    public Guid PersonId { get; set; }

    public string ContractNumber { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Recruitment Recruitment { get; set; } = null!;
}
