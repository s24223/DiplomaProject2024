using Domain.Features.Characteristic.Entities;

namespace Application.Shared.DTOs.Features.Characteristics.Responses
{
    public class QualityResponseDto
    {
        //Values
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;


        //Constructor
        public QualityResponseDto(DomainQuality domain)
        {
            Id = domain.Id.Value;
            Name = domain.Name;
            Description = domain.Description;
        }
    }
}
