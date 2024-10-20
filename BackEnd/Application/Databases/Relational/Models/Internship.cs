namespace Application.Databases.Relational.Models;

public partial class Internship
{
    public Guid Id { get; set; }

    public string ContractNumber { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateOnly ContractStartDate { get; set; }

    public DateOnly? ContractEndDate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Recruitment Recruitment { get; set; } = null!;
}
