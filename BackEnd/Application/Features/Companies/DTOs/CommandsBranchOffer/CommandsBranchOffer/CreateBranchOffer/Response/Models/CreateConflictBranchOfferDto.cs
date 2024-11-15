using Domain.Features.BranchOffer.Entities;

namespace Application.Features.Companies.DTOs.CommandsBranchOffer.CommandsBranchOffer.CreateBranchOffer.Response.Models
{
    public class CreateConflictBranchOfferDto
    {
        //Values
        public CreateIncorrectBranchOfferDto Key { get; private set; }
        public IEnumerable<CreateIncorrectBranchOfferDto> Values { get; private set; }


        //Cosntructor
        public CreateConflictBranchOfferDto
            (
            DomainBranchOffer key,
            IEnumerable<DomainBranchOffer> values
            )
        {
            Key = new CreateIncorrectBranchOfferDto(key);
            Values = values.Select(x => new CreateIncorrectBranchOfferDto(x));
        }
    }
}
