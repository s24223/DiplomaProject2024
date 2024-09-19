namespace Application.Database.Models;

public partial class Comment
{
    public Guid InternshipId { get; set; }

    public int CommentTypeId { get; set; }

    public DateTime Published { get; set; }

    public string Description { get; set; } = null!;

    public int? Evaluation { get; set; }

    public virtual CommentType CommentType { get; set; } = null!;

    public virtual Internship Internship { get; set; } = null!;
}
