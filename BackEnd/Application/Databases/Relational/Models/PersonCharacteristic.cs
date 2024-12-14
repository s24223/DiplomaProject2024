namespace Application.Databases.Relational.Models;

public partial class PersonCharacteristic
{
    public Guid PersonId { get; set; }

    public int CharacteristicId { get; set; }

    public int? QualityId { get; set; }

    public virtual Characteristic Characteristic { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Quality? Quality { get; set; }
}
