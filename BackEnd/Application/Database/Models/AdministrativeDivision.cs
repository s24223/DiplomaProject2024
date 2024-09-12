using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class AdministrativeDivision
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentDivisionId { get; set; }

    public int CountryId { get; set; }

    public int AdministrativeTypeId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual AdministrativeType AdministrativeType { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<AdministrativeDivision> InverseParentDivision { get; set; } = new List<AdministrativeDivision>();

    public virtual AdministrativeDivision? ParentDivision { get; set; }

    public virtual ICollection<Street> Streets { get; set; } = new List<Street>();
}
