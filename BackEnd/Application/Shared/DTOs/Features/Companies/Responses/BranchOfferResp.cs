using Domain.Features.BranchOffer.Entities;
using Domain.Shared.ValueObjects;

namespace Application.Shared.DTOs.Features.Companies.Responses
{
    public class BranchOfferResp
    {
        //Values
        public Guid Id { get; init; }
        public Guid BranchId { get; init; }
        public Guid OfferId { get; init; }
        public DateTime Created { get; init; }
        public DateTime PublishStart { get; init; }
        public DateTime? PublishEnd { get; set; }
        public DateOnly? WorkStart { get; set; }
        public DateOnly? WorkEnd { get; set; }
        public Duration? WorkDuration { get; set; }
        public DateTime LastUpdate { get; set; }


        //Cosntructor
        public BranchOfferResp(DomainBranchOffer domain)
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
