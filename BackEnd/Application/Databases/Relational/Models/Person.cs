using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class Person
{
    public Guid UserId { get; set; }

    public Guid? AddressId { get; set; }

    public DateOnly Created { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhoneNum { get; set; }

    public string? UrlSegment { get; set; }

    public string? LogoUrl { get; set; }

    public string? Description { get; set; }

    public string IsStudent { get; set; } = null!;

    public string IsPublicProfile { get; set; } = null!;

    public virtual Address? Address { get; set; }

    public virtual ICollection<PersonCharacteristic> PersonCharacteristics { get; set; } = new List<PersonCharacteristic>();

    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();

    public virtual User User { get; set; } = null!;
}
