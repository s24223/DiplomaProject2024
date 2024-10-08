using Domain.Features.Address.ValueObjects.Identificators;

namespace Domain.Features.Address.Entities
{
    public class DomainStreet
    {
        //Values
        public StreetId Id { get; private set; }
        public string Name { get; private set; } = null!;


        //References
        public DomainAdministrativeType? StreetType { get; private set; } = null!;


        //Cosntructor
        public DomainStreet
            (
            int id,
            string name,
            int? streetTypeId,
            string? streetTypeName
            )
        {
            Id = new StreetId(id);
            Name = name;
            StreetType = streetTypeId == null || string.IsNullOrWhiteSpace(streetTypeName) ?
                null : new DomainAdministrativeType(streetTypeId.Value, streetTypeName);
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
    }
}
