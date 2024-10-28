using Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer;
using Application.Features.Companies.Interfaces.QueriesCompany;
using Application.Shared.DTOs.Response;
using Domain.Shared.Providers;

namespace Application.Features.Companies.Services.QueriesCompany
{
    public class CompanyQueryService : ICompanyQueryService
    {
        //Values 
        private readonly IProvider _provider;
        private readonly ICompanyQueryRepository _repository;


        //Constructor
        public CompanyQueryService
            (
            IProvider provider,
            ICompanyQueryRepository repository
            )
        {
            _provider = provider;
            _repository = repository;
        }


        //============================================================================
        //============================================================================
        //============================================================================
        //Public Methods
        public async Task<ResponseItems<OfferQueryResponseDto>> GetOffersAsync
            (
            string orderBy = "publishStart",
            bool ascending = true,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            Guid? companyId = null,
            Guid? branchId = null,
            Guid? offerId = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            int? divisionId = null,
            int maxItems = 100,
            int page = 1,
            CancellationToken cancellation = default
            )
        {
            var now = _provider.TimeProvider().GetDateTimeNow();
            var todayDateOnly = _provider.TimeProvider().GetDateOnlyToday();

            //Salary Part
            salaryMin = (salaryMin <= 0) ? null : salaryMin;
            salaryMax = (salaryMax <= 0) ? null : salaryMax;
            var isPayedValidate = (isPayed == null) ? (bool)false : isPayed.Value;

            //Time Part
            publishStart = (publishStart is not null && publishStart >= now) ? now : publishStart;
            publishEnd = (publishEnd is not null && publishEnd <= now) ? now : publishEnd;

            var workStartValidate = (workStart is null) ?
                (DateOnly?)null : _provider.TimeProvider().ToDateOnly(workStart.Value);
            workStartValidate = (workStartValidate < todayDateOnly) ? todayDateOnly : workStartValidate;

            var workEndValidate = (workEnd is null) ?
                (DateOnly?)null : _provider.TimeProvider().ToDateOnly(workEnd.Value);
            workEndValidate = (workEndValidate < todayDateOnly) ? todayDateOnly : workEndValidate;


            maxItems = (maxItems < 10 || maxItems > 100) ? 100 : maxItems;
            page = (page < 1) ? 1 : page;

            var list = await _repository.GetOffersAsync
                (
                orderBy,
                ascending,
                isPayedValidate,
                publishStart,
                publishEnd,
                workStartValidate,
                workEndValidate,
                companyId,
                branchId,
                offerId,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                divisionId,
                maxItems,
                page,
                cancellation
                );


            return new ResponseItems<OfferQueryResponseDto>
            {
                Items = list.Select(x => new OfferQueryResponseDto(x)).ToList(),
            };
        }
        //============================================================================
        //============================================================================
        //============================================================================
        //Private Methods
    }
}
