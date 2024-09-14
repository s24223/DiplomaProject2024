using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string LoginEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public DateTime? ExpiredToken { get; set; }

    public DateTime? LastLoginIn { get; set; }

    public DateTime LastUpdatePassword { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();

    public virtual ICollection<UserProblem> UserProblems { get; set; } = new List<UserProblem>();
}
