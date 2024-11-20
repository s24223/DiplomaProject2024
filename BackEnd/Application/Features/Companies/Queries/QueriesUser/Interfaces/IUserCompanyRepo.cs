using Domain.Features.Branch.Entities;
using Domain.Features.Offer.Entities;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Features.Companies.Queries.QueriesUser.Interfaces
{
    public interface IUserCompanyRepo
    {
        Task<(int TotalCount, IEnumerable<DomainBranch> Items)> GetCoreBranchesAsync
            (
            UserId companyId,
            CancellationToken cancellation,
            int? divisionId = null,
            int? streetId = null,
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            );

        Task<(int TotalCount, IEnumerable<DomainOffer> Items)> GetCoreOffersAsync
            (
            UserId comapnyId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            string? searchText = null,
            bool? isNegotiatedSalary = null,
            bool? isForStudents = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            );
    }
}
