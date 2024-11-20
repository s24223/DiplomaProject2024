using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.Commands.BranchOffers.DTOs.CommandsBranchOffer.UpdateBranchOffer.Response.Models
{
    public class UpdateConflictBranchOfferDto
    {
        //Values
        public BranchOfferResp Key { get; private set; }
        public IEnumerable<BranchOfferResp> Values { get; private set; }


        //Cosntructor
        public UpdateConflictBranchOfferDto
            (
            DomainBranchOffer key,
            IEnumerable<DomainBranchOffer> values
            )
        {
            Key = new BranchOfferResp(key);
            Values = values.Select(x => new BranchOfferResp(x));
        }
    }
}
