namespace Application.Databases.Relational.Models;

public partial class Recruitment
{
    public Guid PersonId { get; set; }

    public Guid BranchOfferId { get; set; }

    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public string? CvUrl { get; set; }

    public string? PersonMessage { get; set; }

    public string? CompanyResponse { get; set; }

    public string? IsAccepted { get; set; }

    public virtual BranchOffer BranchOffer { get; set; } = null!;

    public virtual Internship? Internship { get; set; }

    public virtual Person Person { get; set; } = null!;
}
