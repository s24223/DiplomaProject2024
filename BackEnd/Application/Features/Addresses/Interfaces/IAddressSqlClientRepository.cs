using Application.Databases.Relational.Models;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Interfaces
{
    public interface IAddressSqlClientRepository
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
    }
}
