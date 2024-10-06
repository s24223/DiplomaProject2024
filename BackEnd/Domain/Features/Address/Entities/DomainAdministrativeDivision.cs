namespace Domain.Features.Address.Entities
{
    public class DomainAdministrativeDivision
    {
        public int Id { get; private set; }
        public string AdministrativeDivisionName { get; private set; } = null!;
        public int? ParentDivisionId { get; private set; } = null;
        public int AdministrativeTypeId { get; private set; }
        public string AdministrativeTypeName { get; private set; } = null!;

        public DomainAdministrativeDivision
            (
            int id,
            string administrativeDivisionName,
            int? parentDivisionId,
            int administrativeTypeId,
            string administrativeTypeName
            )
        {
            Id = id;
            AdministrativeDivisionName = administrativeDivisionName;
            ParentDivisionId = parentDivisionId;
            AdministrativeTypeId = administrativeTypeId;
            AdministrativeTypeName = administrativeTypeName;
        }
    }
}
