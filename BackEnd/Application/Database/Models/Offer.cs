namespace Application.Database.Models;

public partial class Offer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string? NegotiatedSalary { get; set; }

    public string ForStudents { get; set; } = null!;

    public virtual ICollection<BranchOffer> BranchOffers { get; set; } = new List<BranchOffer>();

    public virtual ICollection<OfferCharacteristicsList> OfferCharacteristicsLists { get; set; } = new List<OfferCharacteristicsList>();
}
