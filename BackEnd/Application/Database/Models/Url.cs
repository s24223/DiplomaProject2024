using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Url
{
    public Guid UserId { get; set; }

    public int TypeUrlid { get; set; }

    public DateTime PublishDate { get; set; }

    public string Description { get; set; } = null!;

    public virtual TypeUrl TypeUrl { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
