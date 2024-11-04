using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Interfaces.QueriesOffer
{
    public interface IOfferQueryRepository
    {
        Task<DomainOffer> GetOfferAsync
            (
            OfferId id,
            CancellationToken cancellation,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            string orderBy,
            bool ascending,
            int maxItems = 100,
            int page = 1
            );

        Task<IEnumerable<DomainBranchOffer>> GetOffersAsync
            (
            UserId? companyId,
            DivisionId? divisionId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary,
            string orderBy,
            bool ascending,
            int maxItems = 100,
            int page = 1
            );

        Task<DomainBranch> GetOffersByBranchAsync
           (
            UserId? companyId,
            string? companyUrlsegment,
            BranchId? branchId,
            string? branchUrlSegment,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool isPayed,
            DateTime? publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            decimal? salaryMin,
            decimal? salaryMax,
            bool? isNegotietedSalary,
            string orderBy,
            bool ascending,
            int maxItems = 100,
            int page = 1
           );
        /*
                Task<DomainCompany> GetOffersByCompanyAsync
                   (
                    UserId? companyId,
                    string? companyUrlsegment,
                    BranchId? branchId,
                    DivisionId? divisionId,
                    CancellationToken cancellation,
                    bool isPayed,
                    DateTime? publishStart,
                    DateTime? publishEnd,
                    DateOnly? workStart,
                    DateOnly? workEnd,
                    decimal? salaryMin,
                    decimal? salaryMax,
                    bool? isNegotietedSalary,
                    string orderBy,
                    bool ascending,
                    int maxItems = 100,
                    int page = 1
                   );*/

    }
}
