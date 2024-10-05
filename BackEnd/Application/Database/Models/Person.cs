using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Person
{
    public Guid UserId { get; set; }

    public Guid? AddressId { get; set; }

    public DateOnly Created { get; set; }

    public byte[]? Logo { get; set; }

    public string? UrlSegment { get; set; }

    public string ContactEmail { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string? ContactPhoneNum { get; set; }

    public string? Description { get; set; }

    public string IsStudent { get; set; } = null!;

    public string IsPublicProfile { get; set; } = null!;

    public virtual Address? Address { get; set; }

    public virtual ICollection<PersonCharacteristicsList> PersonCharacteristicsLists { get; set; } = new List<PersonCharacteristicsList>();

    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();

    public virtual User User { get; set; } = null!;
}
