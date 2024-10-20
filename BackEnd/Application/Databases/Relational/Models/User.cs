using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? Login { get; set; }

    public string Salt { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? CreatedProfileUrlSegment { get; set; }

    public DateTime LastPasswordUpdate { get; set; }

    public DateTime? LastLoginIn { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? ExpiredToken { get; set; }

    public DateTime? ResetPasswordInitiated { get; set; }

    public string? ResetPasswordUrlSegment { get; set; }

    public string IsHideProfile { get; set; } = null!;

    public virtual Company? Company { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Person? Person { get; set; }

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
}
