using Application.Database.Models;

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

        Task<IEnumerable<AdministrativeDivision>> GetDivisionsHierachyUpAsync
            (
            int divisionId,
            CancellationToken cancellation
            );
    }
}
