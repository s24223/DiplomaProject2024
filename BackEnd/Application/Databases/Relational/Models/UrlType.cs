using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class UrlType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
}
