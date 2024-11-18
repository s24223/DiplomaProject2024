using Application.Shared.DTOs.Features.Characteristics.Responses;
using Domain.Features.Offer.Entities;

namespace Application.Shared.DTOs.Features.Companies.Responses
{
    public class OfferResponseDto
    {
        //Values
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool? IsNegotiatedSalary { get; set; }
        public bool IsForStudents { get; set; }
        public bool IsPaid { get; set; }
        public IEnumerable<CharCollocationResp> Characteristics { get; set; }
            = new List<CharCollocationResp>();


        //Cosnstructor
        public OfferResponseDto(DomainOffer domain)
        {
            Id = domain.Id.Value;
            Name = domain.Name;
            Description = domain.Description;
            MinSalary = domain.MinSalary;
            MaxSalary = domain.MaxSalary;
            IsNegotiatedSalary = domain.IsNegotiatedSalary;
            IsForStudents = domain.IsForStudents;
            IsPaid = domain.IsPaid;
            Characteristics = domain.Characteristics.Values.Select(x =>
            new CharCollocationResp(x.Item1, x.Item2));
        }
    }
}
