using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class Characteristic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CharacteristicTypeId { get; set; }

    public virtual ICollection<BranchCharacteristic> BranchCharacteristics { get; set; } = new List<BranchCharacteristic>();

    public virtual CharacteristicType CharacteristicType { get; set; } = null!;

    public virtual ICollection<OfferCharacteristic> OfferCharacteristics { get; set; } = new List<OfferCharacteristic>();

    public virtual ICollection<PersonCharacteristic> PersonCharacteristics { get; set; } = new List<PersonCharacteristic>();

    public virtual ICollection<Characteristic> ChildCharacteristics { get; set; } = new List<Characteristic>();

    public virtual ICollection<Characteristic> ParentCharacteristics { get; set; } = new List<Characteristic>();
}
