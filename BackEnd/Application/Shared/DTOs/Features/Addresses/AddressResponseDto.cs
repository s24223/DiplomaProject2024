using Domain.Features.Address.Entities;

namespace Application.Shared.DTOs.Features.Addresses
{
    public class AddressResponseDto
    {
        //Values
        public Guid Id { get; set; }
        public int DivisionId { get; private set; }
        public IEnumerable<DivisionResponseDto> Hierarchy { get; private set; } = new List<DivisionResponseDto>();
        public int? StreetId { get; private set; }
        public StreetResponseDto? Street { get; private set; }
        public string BuildingNumber { get; private set; } = null!;
        public string? ApartmentNumber { get; private set; }
        public string ZipCode { get; private set; } = null!;
        public double Lon { get; private set; } = 0;
        public double Lat { get; private set; } = 0;


        //Constructor
        public AddressResponseDto(DomainAddress address)
        {
            Id = address.Id.Value;

            DivisionId = address.DivisionId.Value;
            BuildingNumber = (string)address.BuildingNumber;
            ApartmentNumber = (string?)address.ApartmentNumber;
            ZipCode = (string)address.ZipCode;
            Lon = address.Lon;
            Lat = address.Lat;
            if (address.Street != null)
            {
                StreetId = address.StreetId?.Value;
                Street = new StreetResponseDto
                {
                    Id = address.Street.Id.Value,
                    Name = address.Street.Name,
                    AdministrativeType = address.Street.StreetType == null ?
                    null : new AdministrativeTypeResponseDto
                    {
                        Id = address.Street.StreetType.Id,
                        Name = address.Street.StreetType.Name,
                    }
                };
            }

            Hierarchy = address.Hierarchy.Select(x => new DivisionResponseDto
            {
                Id = x.Id.Value,
                Name = x.Name,
                ParentId = x.ParentDivisionId,
                AdministrativeType = new AdministrativeTypeResponseDto
                {
                    Id = x.DivisionType.Id,
                    Name = x.DivisionType.Name,
                }
            }).ToList();
        }
    }
}
