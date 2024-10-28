using Application.Databases.Relational.Models;
using Application.Shared.DTOs.Features.Addresses;

namespace Application.Features.Addresses.DTOs.Queries
{
    public class DivisionStreetsResponseDto
    {
        //Cosntructor
        public DivisionResponseDto Division { get; set; }
        private IEnumerable<StreetResponseDto> _streets = new List<StreetResponseDto>();
        public IEnumerable<StreetResponseDto> Streets
        {
            get { return _streets; }
            set
            {
                Count = value.Count();
                _streets = value;
            }
        }
        public int Count { get; private set; }


        //Cosntructor
        public DivisionStreetsResponseDto(AdministrativeDivision division)
        {
            Division = new DivisionResponseDto
            {
                Id = division.Id,
                Name = division.Name,
                ParentId = division.ParentDivisionId,
                AdministrativeType = new AdministrativeTypeResponseDto
                {
                    Id = division.AdministrativeType.Id,
                    Name = division.AdministrativeType.Name,
                },
            };

            Streets = division.Streets.Select(y => new StreetResponseDto
            {
                Id = y.Id,
                Name = y.Name,
                AdministrativeType = y.AdministrativeType == null ? null : new AdministrativeTypeResponseDto
                {
                    Id = y.AdministrativeType.Id,
                    Name = y.AdministrativeType.Name,
                }
            }).ToList();
        }
    }
}
