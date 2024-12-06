using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class Quality
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? CharacteristicTypeId { get; set; }

    public virtual CharacteristicType? CharacteristicType { get; set; }

    public virtual ICollection<OfferCharacteristic> OfferCharacteristics { get; set; } = new List<OfferCharacteristic>();

    public virtual ICollection<PersonCharacteristic> PersonCharacteristics { get; set; } = new List<PersonCharacteristic>();
}
