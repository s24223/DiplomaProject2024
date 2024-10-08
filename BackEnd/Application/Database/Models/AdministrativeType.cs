namespace Application.Database.Models;

public partial class AdministrativeType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AdministrativeDivision> AdministrativeDivisions { get; set; } = new List<AdministrativeDivision>();

    public virtual ICollection<Street> Streets { get; set; } = new List<Street>();
}
