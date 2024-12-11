﻿using Application.Databases.Relational.Models;
using Application.Shared.DTOs.Features.Internships;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.ValueObjects.Identificators;

namespace Application.Shared.Interfaces.SqlClient
{
    public interface ISqlClientRepo
    {
        Task<IEnumerable<(int DivisionId, Street Street)>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            );

        Task<Dictionary<DivisionId, AdministrativeDivision>> GetDivisionsHierachyUpAsync
            (
            int divisionId,
            CancellationToken cancellation
            );

        Task<(int TotalCount, IEnumerable<Guid> Ids)> GetBranchIdsSorted
            (
            Guid companyId,
            int? divisionId,
            int? streetId,
            int maxItems,
            int page,
            bool ascending,
            CancellationToken cancellation
            );

        Task<IEnumerable<int>> GetDivisionIdsDownAsync
            (
            int divisionId,
            CancellationToken cancellation
            );

        Task<InternshipDetailsResp> GetStatisticDetailsByIntershipAsync
            (
            RecrutmentId recrutmentId,
            UserId userId,
            CancellationToken cancellation
            );
    }
}
