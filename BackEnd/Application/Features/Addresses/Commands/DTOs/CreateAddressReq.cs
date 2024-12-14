using System.ComponentModel.DataAnnotations;

namespace Application.Features.Addresses.Commands.DTOs
{
    public class CreateAddressReq
    {
        [Required]
        public string Country { get; init; } = null!;
        [Required]
        public string State { get; init; } = null!;
        public string? County { get; init; } = null; // Powiat
        public string? Municipality { get; init; } = null; // Gmina
        [Required]
        public string City { get; init; } = null!; // Maisto OR MAisto Na prawach Powiatu
        [Required]
        public string Postcode { get; init; } = null!;
        //public string? District { get; init; } = null; //Ignore
        public string? Suburb { get; init; } = null; // Sometimes: Dzielnica, Delegatura lub dane rundomowe
        public string? Street { get; init; } = null;
        //public string? Building { get; init; } = null;
        //public string? Amenity { get; init; } = null;
        //public string? Unknown { get; init; } = null;


        [Required]
        public double Lon { get; init; } = 0;
        [Required]
        public double Lat { get; init; } = 0;

        [Required]
        public string HouseNumber { get; init; } = null!;
        public string? ApartmentNumber { get; init; }
    }
}
