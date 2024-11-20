using Application.Shared.DTOs.Features.Companies.Responses;
using Domain.Features.Offer.Entities;

namespace Application.Features.Companies.Queries.QueriesUser.DTOs
{
    public class GetCoreOffersResp
    {
        //Values
        public IEnumerable<OfferResp> Offers { get; set; } = [];
        public int Count { get; set; }
        public int TotalCount { get; set; }


        //Constructor
        public GetCoreOffersResp(IEnumerable<DomainOffer> offers, int totalCount)
        {
            Offers = offers.Select(x => new OfferResp(x));
            Count = Offers.Count();
            TotalCount = totalCount;
        }
    }
}
