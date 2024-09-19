namespace Application.Database.Models;

public partial class Characteristic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CharacteristicTypeId { get; set; }

    public virtual ICollection<BranchCharacteristicsList> BranchCharacteristicsLists { get; set; } = new List<BranchCharacteristicsList>();

    public virtual CharacteristicType CharacteristicType { get; set; } = null!;

    public virtual ICollection<OfferCharacteristicsList> OfferCharacteristicsLists { get; set; } = new List<OfferCharacteristicsList>();

    public virtual ICollection<PersonCharacteristicsList> PersonCharacteristicsLists { get; set; } = new List<PersonCharacteristicsList>();

    public virtual ICollection<Characteristic> ChildCharacteristics { get; set; } = new List<Characteristic>();

    public virtual ICollection<Characteristic> ParentCharacteristics { get; set; } = new List<Characteristic>();
}
