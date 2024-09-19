namespace Application.Database.Models;

public partial class Branch
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid AddressId { get; set; }

    public string? UrlSegment { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<BranchCharacteristicsList> BranchCharacteristicsLists { get; set; } = new List<BranchCharacteristicsList>();

    public virtual ICollection<BranchOffer> BranchOffers { get; set; } = new List<BranchOffer>();

    public virtual Company Company { get; set; } = null!;
}
