using Domain.Features.Address.ValueObjects.Identificators;

namespace Domain.Features.Address.Entities
{
    public class DomainAdministrativeDivision
    {
        //Values
        public DivisionId Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public int? ParentDivisionId { get; private set; } = null;


        //References
        public DomainAdministrativeType DivisionType { get; private set; } = null!;


        //Cosntructor
        public DomainAdministrativeDivision
            (
            int id,
            string name,
            int? parentDivisionId,
            int divisionTypeId,
            string divisionTypeName
            )
        {
            Id = new DivisionId(id);
            Name = name;
            ParentDivisionId = parentDivisionId;
            DivisionType = new DomainAdministrativeType
                (
                divisionTypeId,
                divisionTypeName
                );
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
    }
}
