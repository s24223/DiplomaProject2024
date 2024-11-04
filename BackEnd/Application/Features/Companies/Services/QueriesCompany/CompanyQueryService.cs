using Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer;
using Application.Features.Companies.Interfaces.QueriesOffer;
using Application.Shared.DTOs.Response;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;

namespace Application.Features.Companies.Services.QueriesCompany
{
    public class CompanyQueryService : ICompanyQueryService
    {
        //Values 
        private readonly IProvider _provider;
        private readonly IOfferQueryRepository _repository;


        //Constructor
        public CompanyQueryService
            (
            IProvider provider,
            IOfferQueryRepository repository
            )
        {
            _provider = provider;
            _repository = repository;
        }


        //============================================================================
        //============================================================================
        //============================================================================
        //Public Methods
        public async Task<ResponseItem<OfferQueryResponseDto>> GetOfferAsync
            (
            Guid offerId,
            CancellationToken cancellation,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1
            )
        {
            var validateData = DataValidation
                (
                null,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                null,
                null,
                null,
                orderBy,
                maxItems,
                page
                );

            var domain = await _repository.GetOfferAsync
                (
                new OfferId(offerId),
                cancellation,
                validateData.PublishStart,
                validateData.PublishEnd,
                validateData.WorkStart,
                validateData.WorkEnd,
                validateData.OrderBy,
                ascending,
                validateData.MaxItems,
                validateData.Page
                );

            return new ResponseItem<OfferQueryResponseDto>
            {
                Item = new OfferQueryResponseDto(domain)
            };
        }

        public async Task<ResponseItems<BranchOfferQueryResponseDto>> GetOffersAsync
            (
            Guid? companyId,
            int? divisionId,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1
            )
        {

            var validateData = DataValidation
                (
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                orderBy,
                maxItems,
                page
                );


            var list = await _repository.GetOffersAsync
                (
                ReturnUserIdOrNull(companyId),
                ReturnDivisionIdOrNull(divisionId),
                characteristics,
                cancellation,
                validateData.IsPayed,
                validateData.PublishStart,
                validateData.PublishEnd,
                validateData.WorkStart,
                validateData.WorkEnd,
                validateData.SalaryMin,
                validateData.SalaryMax,
                validateData.IsNegotietedSalary,
                validateData.OrderBy,
                ascending,
                validateData.MaxItems
                );


            return new ResponseItems<BranchOfferQueryResponseDto>
            {
                Items = list.Select(x => new BranchOfferQueryResponseDto(x)).ToList(),
            };
        }



        public async Task<ResponseItem<BranchQueryResponseDto>> GetOffersByBranchAsync
            (
            Guid? companyId,
            string? companyUrlsegment,
            Guid? branchId,
            string? branchUrlSegment,
            IEnumerable<int> characteristics,
            CancellationToken cancellation,
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            string orderBy = "publishStart",
            bool ascending = true,
            int maxItems = 100,
            int page = 1
            )
        {
            var validateData = DataValidation
                (
                isPayed,
                publishStart,
                publishEnd,
                workStart,
                workEnd,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                orderBy,
                maxItems,
                page
                );

            var branch = await _repository.GetOffersByBranchAsync
                (
                ReturnUserIdOrNull(companyId),
                companyUrlsegment,
                ReturnBranchIdOrNull(branchId),
                branchUrlSegment,
                characteristics,
                cancellation,
                validateData.IsPayed,
                validateData.PublishStart,
                validateData.PublishEnd,
                validateData.WorkStart,
                validateData.WorkEnd,
                validateData.SalaryMin,
                validateData.SalaryMax,
                validateData.IsNegotietedSalary,
                validateData.OrderBy,
                ascending,
                validateData.MaxItems
                );


            return new ResponseItem<BranchQueryResponseDto>
            {
                Item = new BranchQueryResponseDto(branch),
            };
        }
        /*
                public async Task<ResponseItem<CompanyOffersQueryResponseDto>> GetOffersByCompanyAsync
                    (
                    Guid? companyId,
                    string? companyUrlsegment,
                    int? divisionId,
                    IEnumerable<int> characteristics,
                    CancellationToken cancellation,
                    bool? isPayed = null,
                    DateTime? publishStart = null,
                    DateTime? publishEnd = null,
                    DateTime? workStart = null,
                    DateTime? workEnd = null,
                    decimal? salaryMin = null,
                    decimal? salaryMax = null,
                    bool? isNegotietedSalary = null,
                    string orderBy = "publishStart",
                    bool ascending = true,
                    int maxItems = 100,
                    int page = 1
                    )
                {
                    var validateData = DataValidation
                        (
                        isPayed,
                        publishStart,
                        publishEnd,
                        workStart,
                        workEnd,
                        salaryMin,
                        salaryMax,
                        isNegotietedSalary,
                        orderBy,
                        maxItems,
                        page
                        );

                    var company = await _repository.GetOffersByCompanyAsync
                        (
                        ReturnUserIdOrNull(companyId),
                        companyUrlsegment,
                        ReturnBranchIdOrNull(branchId),
                        ReturnDivisionIdOrNull(divisionId),
                        cancellation,
                        validateData.IsPayed,
                        validateData.PublishStart,
                        validateData.PublishEnd,
                        validateData.WorkStart,
                        validateData.WorkEnd,
                        validateData.SalaryMin,
                        validateData.SalaryMax,
                        validateData.IsNegotietedSalary,
                        validateData.OrderBy,
                        ascending,
                        validateData.MaxItems
                        );

                    return new ResponseItem<CompanyOffersQueryResponseDto>
                    {
                        Item = new CompanyOffersQueryResponseDto(company),
                    };
                }
        */

        //============================================================================
        //============================================================================
        //============================================================================
        //Private Methods
        private
            (
            bool IsPayed,
            DateTime? PublishStart,
            DateTime? PublishEnd,
            DateOnly? WorkStart,
            DateOnly? WorkEnd,
            decimal? SalaryMin,
            decimal? SalaryMax,
            bool? IsNegotietedSalary,
            string OrderBy,
            int MaxItems,
            int Page
            )
            DataValidation
            (
            bool? isPayed = null,
            DateTime? publishStart = null,
            DateTime? publishEnd = null,
            DateTime? workStart = null,
            DateTime? workEnd = null,
            decimal? salaryMin = null,
            decimal? salaryMax = null,
            bool? isNegotietedSalary = null,
            string orderBy = "publishStart",
            int maxItems = 100,
            int page = 1
            )
        {
            var now = _provider.TimeProvider().GetDateTimeNow();
            var todayDateOnly = _provider.TimeProvider().GetDateOnlyToday();

            //isPayed
            var isPayedValidate = (isPayed == null) ? (bool)false : isPayed.Value;


            //Time Part
            publishStart = (publishStart is not null && publishStart >= now) ? now : publishStart;
            publishEnd = (publishEnd is not null && publishEnd <= now) ? now : publishEnd;
            //workStart
            var workStartValidate = (workStart is null) ?
                (DateOnly?)null : _provider.TimeProvider().ToDateOnly(workStart.Value);
            workStartValidate = (workStartValidate < todayDateOnly) ? todayDateOnly : workStartValidate;
            //workEnd
            var workEndValidate = (workEnd is null) ?
                (DateOnly?)null : _provider.TimeProvider().ToDateOnly(workEnd.Value);
            workEndValidate = (workEndValidate < todayDateOnly) ? todayDateOnly : workEndValidate;


            //Salary Part
            salaryMin = (salaryMin <= 0) ? null : salaryMin;
            salaryMax = (salaryMax <= 0) ? null : salaryMax;


            //Items And Pages
            maxItems = (maxItems < 10 || maxItems > 100) ? 100 : maxItems;
            page = (page < 1) ? 1 : page;

            return
                (
                isPayedValidate,
                publishStart,
                publishEnd,
                workStartValidate,
                workEndValidate,
                salaryMin,
                salaryMax,
                isNegotietedSalary,
                orderBy,
                maxItems,
                page
                );
        }

        private UserId? ReturnUserIdOrNull(Guid? id)
        {
            return id.HasValue ? new UserId(id.Value) : null;
        }

        private BranchId? ReturnBranchIdOrNull(Guid? id)
        {
            return id.HasValue ? new BranchId(id.Value) : null;
        }

        private OfferId? ReturnOfferIdOrNull(Guid? id)
        {
            return id.HasValue ? new OfferId(id.Value) : null;
        }

        private DivisionId? ReturnDivisionIdOrNull(int? id)
        {
            return id.HasValue ? new DivisionId(id.Value) : null;
        }
    }
}
