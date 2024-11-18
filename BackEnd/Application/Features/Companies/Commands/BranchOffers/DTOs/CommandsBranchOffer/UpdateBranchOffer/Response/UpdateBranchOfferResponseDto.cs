using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Response.Models;
using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Response
{
    public class UpdateBranchOfferResponseDto
    {
        //Values
        public IEnumerable<CorrectBranchOfferDto> Correct { get; private set; } = [];
        public int CorrectCount { get; private set; }
        public IEnumerable<BranchOfferResponseDto> WithoutDuration { get; private set; } = [];
        public int WithoutDurationCount { get; private set; }
        public IEnumerable<UpdateConflictBranchOfferDto> Conflicts { get; private set; } = [];


        //Cosntructor
        public UpdateBranchOfferResponseDto
            (
            IEnumerable<DomainBranchOffer> correct,
            IEnumerable<DomainBranchOffer> withoutDuration,
            Dictionary<DomainBranchOffer, List<DomainBranchOffer>> conflicts
            )
        {
            Correct = correct
                .Select(x => new CorrectBranchOfferDto(x));
            CorrectCount = Correct.Count();

            WithoutDuration = withoutDuration
                .Select(x => new BranchOfferResponseDto(x));
            WithoutDurationCount = WithoutDuration.Count();

            Conflicts = conflicts
                .Select(x => new UpdateConflictBranchOfferDto(x.Key, x.Value));
        }
    }
}
