namespace Application.Databases.Relational.Models;

public partial class Address
{
    public Guid Id { get; set; }

    public int DivisionId { get; set; }

    public int? StreetId { get; set; }

    public string BuildingNumber { get; set; } = null!;

    public string? ApartmentNumber { get; set; }

    public string ZipCode { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual AdministrativeDivision Division { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual Street? Street { get; set; }
}
