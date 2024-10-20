using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class Street
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? AdministrativeTypeId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual AdministrativeType? AdministrativeType { get; set; }

    public virtual ICollection<AdministrativeDivision> Divisions { get; set; } = new List<AdministrativeDivision>();
}
