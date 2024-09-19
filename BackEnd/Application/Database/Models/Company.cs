namespace Application.Database.Models;

public partial class Company
{
    public Guid UserId { get; set; }

    public byte[]? Logo { get; set; }

    public string? UrlSegment { get; set; }

    public DateOnly CreateDate { get; set; }

    public string ContactEmail { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Regon { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual User User { get; set; } = null!;
}
