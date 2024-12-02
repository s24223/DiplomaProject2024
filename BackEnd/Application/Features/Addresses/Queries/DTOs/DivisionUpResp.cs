using Application.Databases.Relational.Models;
using Application.Shared.DTOs.Features.Addresses;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Queries.DTOs
{
    public class DivisionUpResp
    {
        //Values
        public IEnumerable<DivisionResponseDto> Hierarchy { get; set; } = [];


        //Constructor
        public DivisionUpResp(Dictionary<DivisionId, AdministrativeDivision> dictionary)
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

        }

        public DivisionUpResp(AdministrativeDivision administrativeDivision)
        {
            var x = new DivisionResponseDto
            {
                Id = administrativeDivision.Id,
                ParentId = administrativeDivision.ParentDivisionId,
                Name = administrativeDivision.Name,
                AdministrativeType = new AdministrativeTypeResponseDto
                {
                    Id = administrativeDivision.AdministrativeType.Id,
                    Name = administrativeDivision.AdministrativeType.Name,
                },
            };
            Hierarchy = new List<DivisionResponseDto> { x };
        }
    }
}
