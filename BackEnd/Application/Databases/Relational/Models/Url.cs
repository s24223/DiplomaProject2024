namespace Application.Databases.Relational.Models;

public partial class Url
{
    public Guid UserId { get; set; }

    public int UrlTypeId { get; set; }

    public DateTime Created { get; set; }

    public string Path { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual UrlType UrlType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
