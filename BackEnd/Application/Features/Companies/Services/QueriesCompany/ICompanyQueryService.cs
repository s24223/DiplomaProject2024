﻿using Application.Features.Companies.DTOs.QueriesCompany.QueriesOffer;
using Application.Shared.DTOs.Response;

namespace Application.Features.Companies.Services.QueriesCompany
{
    public interface ICompanyQueryService
    {
        Task<ResponseItem<OfferQueryResponseDto>> GetOfferAsync
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
            );

        Task<ResponseItems<BranchOfferQueryResponseDto>> GetOffersAsync
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
            );

        Task<ResponseItem<BranchQueryResponseDto>> GetOffersByBranchAsync
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
            );
        /*
                Task<ResponseItem<CompanyOffersQueryResponseDto>> GetOffersByCompanyAsync
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
                    );
        */
    }
}
