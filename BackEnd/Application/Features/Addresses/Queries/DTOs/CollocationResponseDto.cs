using Application.Databases.Relational.Models;
using Application.Shared.DTOs.Features.Addresses;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Queries.DTOs
{
    public class CollocationResponseDto
    {
        //Values
        public IEnumerable<DivisionResponseDto> Hierarchy { get; set; } = new List<DivisionResponseDto>();
        public StreetResponseDto Street { get; set; } = null!;


        //Constructor
        public CollocationResponseDto
            (
            Dictionary<DivisionId, AdministrativeDivision> dictionary,
            Street street
            )
        {
            Hierarchy = dictionary.Values.Select(x => new DivisionResponseDto
            {
                Id = x.Id,
                ParentId = x.ParentDivisionId,
                Name = x.Name,
                AdministrativeType = new AdministrativeTypeResponseDto
                {
                    Id = x.AdministrativeType.Id,
                    Name = x.AdministrativeType.Name,
                },
            });
            Street = new StreetResponseDto
            {
                Id = street.Id,
                Name = street.Name,
                AdministrativeType = street.AdministrativeType == null ?
                    null : new AdministrativeTypeResponseDto
                    {
                        Id = street.AdministrativeType.Id,
                        Name = street.AdministrativeType.Name,
                    },
            };
        }
    }
}
