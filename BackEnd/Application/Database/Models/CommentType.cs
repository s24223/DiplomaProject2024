using System;
using System.Collections.Generic;

namespace Application.Database.Models;

public partial class CommentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
