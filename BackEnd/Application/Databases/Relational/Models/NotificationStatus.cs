using System;
using System.Collections.Generic;

namespace Application.Databases.Relational.Models;

public partial class NotificationStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
