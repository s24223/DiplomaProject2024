namespace Application.Databases.Relational.Models;

public partial class BranchOffer
{
    public Guid Id { get; set; }

    public Guid BranchId { get; set; }

    public Guid OfferId { get; set; }

    public DateTime Created { get; set; }

    public DateTime PublishStart { get; set; }

    public DateTime? PublishEnd { get; set; }

    public DateOnly? WorkStart { get; set; }

    public DateOnly? WorkEnd { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Offer Offer { get; set; } = null!;

    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();
}
