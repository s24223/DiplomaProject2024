using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Exceptions;

namespace Domain.Features.Address.Entities
{
    public class DomainAddress
    {
        //Values
        public AddressId Id { get; private set; }
        public BuildingNumber BuildingNumber { get; private set; } = null!;
        public ApartmentNumber? ApartmentNumber { get; private set; }
        public ZipCode ZipCode { get; private set; } = null!;


        //References
        //Division 
        public DivisionId DivisionId { get; private set; }
        private List<DomainAdministrativeDivision> _hierarchy = new();
        public IReadOnlyCollection<DomainAdministrativeDivision> Hierarchy => _hierarchy;

        //Street 
        public StreetId StreetId { get; private set; }
        private DomainStreet _street = null!;
        public DomainStreet Street
        {
            get { return _street; }
            set
            {
                if (StreetId != value.Id)
                {
                    throw new AddressException
                        (
                        Messages.Address_Street_Invalid,
                        DomainExceptionTypeEnum.AppProblem
                        );
                }
                _street = value;
            }
        }


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
            )
        {
            //Values with exceptions
            ZipCode = new ZipCode(zipCode);
            BuildingNumber = new BuildingNumber(buildingNumber);
            ApartmentNumber = string.IsNullOrWhiteSpace(apartmentNumber) ?
                null : new ApartmentNumber(apartmentNumber);

            //values with no exceptions
            Id = new AddressId(id);
            StreetId = new StreetId(streetId);
            DivisionId = new DivisionId(divisionId);
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public void SetHierarchy(IEnumerable<DomainAdministrativeDivision> hierarchy)
        {
            var divisionWithSameDivisionId = hierarchy
                .Any(x => x.Id == DivisionId);
            if (!divisionWithSameDivisionId)
            {
                throw new AddressException
                    (
                    Messages.Address_DivisionsHierarchy_NotFound,
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            _hierarchy = hierarchy.ToList();
        }

        public void SetZipCode(string zipCode)
        {
            ZipCode = new ZipCode(zipCode);
        }
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
    }
}
