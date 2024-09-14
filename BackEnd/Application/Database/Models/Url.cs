using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Url
{
    public Guid UserId { get; set; }

    public int UrlTypeId { get; set; }

    public DateTime PublishDate { get; set; }

    public string Url1 { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual UrlType UrlType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
