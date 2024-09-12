using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Person
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();

    public virtual User User { get; set; } = null!;
}
