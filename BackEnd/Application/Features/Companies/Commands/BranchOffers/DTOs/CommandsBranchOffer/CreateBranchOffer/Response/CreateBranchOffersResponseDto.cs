using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer;
using Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Response.Models;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.CreateBranchOffer.Response
{
    public class CreateBranchOffersResponseDto
    {
        //Values
        public IEnumerable<CorrectBranchOfferDto> Correct { get; private set; } = [];
        public int CorrectCount { get; private set; }
        public IEnumerable<CreateIncorrectBranchOfferDto> WithoutDuration { get; private set; } = [];
        public int WithoutDurationCount { get; private set; }
        public IEnumerable<CreateConflictBranchOfferDto> Conflicts { get; private set; } = [];


        //Cosntructor
        public CreateBranchOffersResponseDto
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
                .Select(x => new CreateIncorrectBranchOfferDto(x));
            WithoutDurationCount = WithoutDuration.Count();

            Conflicts = conflicts
                .Select(x => new CreateConflictBranchOfferDto(x.Key, x.Value));
        }
    }
}
