using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class Quality
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? CharacteristicTypeId { get; set; }

    public virtual ICollection<BranchCharacteristicsList> BranchCharacteristicsLists { get; set; } = new List<BranchCharacteristicsList>();

    public virtual CharacteristicType? CharacteristicType { get; set; }

    public virtual ICollection<OfferCharacteristicsList> OfferCharacteristicsLists { get; set; } = new List<OfferCharacteristicsList>();

    public virtual ICollection<PersonCharacteristicsList> PersonCharacteristicsLists { get; set; } = new List<PersonCharacteristicsList>();
}
