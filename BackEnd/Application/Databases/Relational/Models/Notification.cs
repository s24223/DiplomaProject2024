namespace Application.Databases.Relational.Models;

public partial class Notification
{
    public Guid? UserId { get; set; }

    public string? Email { get; set; }

    public int NotificationSenderId { get; set; }

    public int NotificationStatusId { get; set; }

    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Completed { get; set; }

    public Guid? PreviousProblemId { get; set; }

    public Guid? IdAppProblem { get; set; }

    public string? UserMessage { get; set; }

    public string? Response { get; set; }

    public string IsReadedByUser { get; set; } = null!;

    public virtual NotificationSender NotificationSender { get; set; } = null!;

    public virtual NotificationStatus NotificationStatus { get; set; } = null!;

    public virtual User? User { get; set; }
}
