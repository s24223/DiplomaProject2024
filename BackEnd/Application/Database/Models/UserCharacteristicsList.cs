namespace Application.Database.Models;

public partial class UserCharacteristicsList
{
    public Guid UserId { get; set; }

    public int QualityId { get; set; }

    public int CharacteristicId { get; set; }

    public virtual Characteristic Characteristic { get; set; } = null!;

    public virtual Quality Quality { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
