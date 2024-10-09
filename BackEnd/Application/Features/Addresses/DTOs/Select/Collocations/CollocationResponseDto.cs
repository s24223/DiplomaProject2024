using Application.Database.Models;
using Application.Features.Addresses.DTOs.Select.Shared;

namespace Application.Features.Addresses.DTOs.Select.Collocations
{
    public class CollocationResponseDto
    {
        //Values
        public IEnumerable<DivisionResponseDto> Hierarchy { get; set; } = new List<DivisionResponseDto>();
        public StreetResponseDto Street { get; set; } = null!;


        //Constructor
        public CollocationResponseDto
            (
            IEnumerable<AdministrativeDivision> hierarchy,
            Street street
            )
        {
            Hierarchy = hierarchy.ToList().Select(x => new DivisionResponseDto
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
