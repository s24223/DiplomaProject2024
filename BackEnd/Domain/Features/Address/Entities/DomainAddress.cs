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
        public double Lon { get; private set; } = 0;
        public double Lat { get; private set; } = 0;


        //References
        //Division 
        public DivisionId DivisionId { get; private set; }
        private List<DomainAdministrativeDivision> _hierarchy = [];
        public IReadOnlyCollection<DomainAdministrativeDivision> Hierarchy => _hierarchy;

        //Street 
        public StreetId? StreetId { get; private set; }
        private DomainStreet? _street = null!;
        public DomainStreet? Street
        {
            get { return _street; }
            set
            {
                if (StreetId != null && value != null)
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
        }


        //Constructor
        public DomainAddress(
            Guid? id,
            int divisionId,
            int? streetId,
            string buildingNumber,
            string? apartmentNumber,
            string zipCode,
            double lon,
            double lat,
            IProvider provider)
        {
            //Values with exceptions
            ZipCode = (ZipCode)zipCode;
            BuildingNumber = (BuildingNumber)buildingNumber;
            ApartmentNumber = (ApartmentNumber?)apartmentNumber;

            //values with no exceptions
            Id = new AddressId(id);
            StreetId = !streetId.HasValue ? null : new StreetId(streetId.Value);
            DivisionId = new DivisionId(divisionId);
            Lon = lon;
            Lat = lat;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public void SetHierarchy(Dictionary<DivisionId, DomainAdministrativeDivision> databseDictionary)
        {
            if (!databseDictionary.ContainsKey(DivisionId))
            {
                throw new AddressException
                    (
                    Messages.Address_DivisionsHierarchy_NotFound,
                    DomainExceptionTypeEnum.AppProblem
                    );
            }
            _hierarchy = databseDictionary.Values.ToList();
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
    }
}
