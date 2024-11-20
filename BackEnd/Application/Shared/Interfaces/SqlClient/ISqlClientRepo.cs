using Application.Databases.Relational.Models;
using Domain.Features.Address.ValueObjects.Identificators;

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
    }
}
