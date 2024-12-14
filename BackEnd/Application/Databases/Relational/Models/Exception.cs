namespace Application.Databases.Relational.Models;

public partial class Exception
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public string ExceptionType { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? AdditionalData { get; set; }

    public string Status { get; set; } = null!;
}
