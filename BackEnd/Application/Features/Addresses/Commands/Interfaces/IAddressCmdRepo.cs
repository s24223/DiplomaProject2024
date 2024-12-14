using Domain.Features.Address.ValueObjects;

namespace Application.Features.Addresses.Commands.Interfaces
{
    public interface IAddressCmdRepo
    {
        Task<Guid> CreateAsync
            (
            string wojewodztwo,
            string? powiat,
            string? gmina,
            string city,
            string? dzielnica,
            string? street,
            double lon,
            double lat,
            ZipCode postcode,
            BuildingNumber houseNumber,
            ApartmentNumber? apartmentNumber,
            CancellationToken cancellation
            );
    }
}
