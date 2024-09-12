using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public byte[]? Logo { get; set; }

    public DateTime CeateDate { get; set; }

    public string? Description { get; set; }

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime ExpiredToken { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();

    public virtual ICollection<UserCharacteristicsList> UserCharacteristicsLists { get; set; } = new List<UserCharacteristicsList>();
}
