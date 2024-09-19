namespace Application.Database.Models;

public partial class UserProblem
{
    public Guid Id { get; set; }

    public DateTime DateTime { get; set; }

    public string UserMessage { get; set; } = null!;

    public string? Response { get; set; }

    public Guid? PreviousProblemId { get; set; }

    public Guid? UserId { get; set; }

    public string? Email { get; set; }

    public string Status { get; set; } = null!;

    public virtual User? User { get; set; }
}
