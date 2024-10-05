using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.VerticalSlice.AddressPart.ValueObjects;
using Domain.VerticalSlice.AddressPart.ValueObjects.Identificators;

namespace Domain.VerticalSlice.AddressPart.Entities
{
    public class DomainAddress : Entity<AddressId>
    {
        //Values
        public DivisionId DivisionId { get; private set; }
        public StreetId StreetId { get; private set; }
        public BuildingNumber BuildingNumber { get; private set; } = null!;
        public ApartmentNumber? ApartmentNumber { get; private set; }
        public ZipCode ZipCode { get; set; } = null!;


        //References
        private List<DomainAdministrativeDivision> _hierarchy = new();
        public IReadOnlyCollection<DomainAdministrativeDivision> Hierarchy => _hierarchy;


        //Constructor
        public DomainAddress
            (
            Guid? id,
            int divisionId,
            int streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode,
            IProvider provider
            ) : base(new AddressId(id), provider)
        {
            //Values with exceptions

            BuildingNumber = new BuildingNumber(buildingNumber);
            ApartmentNumber = string.IsNullOrWhiteSpace(apartmentNumber) ?
                null : new ApartmentNumber(apartmentNumber);
            ZipCode = new ZipCode(zipCode);

            //values with no exceptions
            DivisionId = new DivisionId(divisionId);
            StreetId = new StreetId(streetId);
        }
    }
}
