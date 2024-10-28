namespace Application.Databases.Relational.Models;

public partial class Company
{
    public Guid UserId { get; set; }

    public DateOnly Created { get; set; }

    public string? Name { get; set; }

    public string? Regon { get; set; }

    public string? ContactEmail { get; set; }

    public string? UrlSegment { get; set; }

    public string? LogoUrl { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual User User { get; set; } = null!;
}
