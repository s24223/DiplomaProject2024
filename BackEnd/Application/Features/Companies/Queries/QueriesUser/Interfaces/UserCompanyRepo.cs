using Application.Databases.Relational;
using Application.Features.Companies.Mappers;
using Domain.Features.Branch.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;

namespace Application.Features.Companies.Queries.QueriesUser.Interfaces
{
    public class UserCompanyRepo : IUserCompanyRepo
    {
        //Values
        private readonly IProvider _provider;
        private readonly ICompanyMapper _companyMapper;
        private readonly DiplomaProjectContext _context;

        //Constructor 
        public UserCompanyRepo
            (
            IProvider provider,
            ICompanyMapper companyMapper,
            DiplomaProjectContext context
            )
        {
            _provider = provider;
            _companyMapper = companyMapper;
            _context = context;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public async Task<(int TotalCount, IEnumerable<DomainBranch> Items)> GetBranchesAsync
            (
            UserId comapnyId,
            CancellationToken cancellation,
            string? searchText = null,
            string orderBy = "created",
            bool ascending = true,
            int itemsCount = 100,
            int page = 1
            )
        {
            //Adapt data 
            itemsCount = (itemsCount < 10) ? 10 : itemsCount;
            itemsCount = (itemsCount > 100) ? 100 : itemsCount;
            page = (page < 1) ? 1 : page;
            orderBy = orderBy.Trim().ToLower();


            throw new NotImplementedException();
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
    }
}
