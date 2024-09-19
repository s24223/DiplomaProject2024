namespace Application.Database.Models;

public partial class PersonCharacteristicsList
{
    public int CharacteristicId { get; set; }

    public Guid PersonId { get; set; }

    public int? QualityId { get; set; }

    public virtual Characteristic Characteristic { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Quality? Quality { get; set; }
}
