using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class BranchCharacteristicsList
{
    public Guid BranchId { get; set; }

    public int QualityId { get; set; }

    public int CharacteristicId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Characteristic Characteristic { get; set; } = null!;

    public virtual Quality Quality { get; set; } = null!;
}
