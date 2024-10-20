using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class CharacteristicType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();

    public virtual ICollection<Quality> Qualities { get; set; } = new List<Quality>();
}
