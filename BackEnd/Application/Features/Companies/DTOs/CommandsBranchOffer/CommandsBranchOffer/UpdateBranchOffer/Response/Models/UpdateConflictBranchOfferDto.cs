using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.UpdateBranchOffer.Response.Models
{
    public class UpdateConflictBranchOfferDto
    {
        //Values
        public BranchOfferResponseDto Key { get; private set; }
        public IEnumerable<BranchOfferResponseDto> Values { get; private set; }


        //Cosntructor
        public UpdateConflictBranchOfferDto
            (
            DomainBranchOffer key,
            IEnumerable<DomainBranchOffer> values
            )
        {
            Key = new BranchOfferResponseDto(key);
            Values = values.Select(x => new BranchOfferResponseDto(x));
        }
    }
}
