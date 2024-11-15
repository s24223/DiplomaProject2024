using Domain.Features.BranchOffer.Entities;
using Domain.Shared.ValueObjects;

namespace Application.Shared.DTOs.Features.Companies.Responses
{
    public class BranchOfferResponseDto
    {
        //Values
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public Guid OfferId { get; set; }
        public DateTime Created { get; set; }
        public DateTime PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
        public Duration? WorkDuration { get; set; }
        public DateTime LastUpdate { get; set; }


        //Cosntructor
        public BranchOfferResponseDto(DomainBranchOffer domain)
        {
            Id = domain.Id.Value;
            BranchId = domain.BranchId.Value;
            OfferId = domain.OfferId.Value;
            Created = domain.Created;
            PublishStart = domain.PublishStart;
            PublishEnd = domain.PublishEnd;
            WorkStart = domain.WorkStart;
            WorkEnd = domain.WorkEnd;
            WorkDuration = domain.WorkDuration;
            LastUpdate = domain.LastUpdate;
        }
    }
}
