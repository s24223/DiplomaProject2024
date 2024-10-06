using Application.Database;
using Application.Features.Address.DTOs.Select;
using Domain.Shared.Providers;

namespace Application.Features.Address.Interfaces
{
    public class AddressRepository : IAddressRepository
    {
        //Values
        private readonly IProvider _provider;
        private readonly IAddressSqlClientRepository _sql;
        private readonly DiplomaProjectContext _context;


        //Cosntructor
        public AddressRepository
            (
            IProvider provider,
            IAddressSqlClientRepository sql,
            DiplomaProjectContext context
            )
        {
            _sql = sql;
            _provider = provider;
            _context = context;
        }


        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Public Methods
        public async Task<IEnumerable<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            var collocations = new List<CollocationResponseDto>();

            var list = await _sql.GetCollocationsAsync(divisionName, streetName, cancellation);
            foreach (var item in list)
            {
                var hierarchy = await _sql.GetDivisionsHierachyUpAsync
                    (
                    item.DivisionId,
                    cancellation
                    );
                var street = item.Street;
                collocations.Add(new CollocationResponseDto
                {
                    Hierarchy = hierarchy.ToList().Select(x => new DivisionResponseDto
                    {
                        Id = x.Id,
                        ParentId = x.ParentDivisionId,
                        Name = x.Name,
                        AdministrativeType = new AdministrativeTypeResponseDto
                        {
                            Id = x.AdministrativeType.Id,
                            Name = x.AdministrativeType.Name,
                        },
                    }),
                    Street = new StreetResponseDto
                    {
                        Id = street.Id,
                        Name = street.Name,
                        AdministrativeType = street.AdministrativeType == null ?
                            null : new AdministrativeTypeResponseDto
                            {
                                Id = street.AdministrativeType.Id,
                                Name = street.AdministrativeType.Name,
                            },
                    },
                });
            }

            return collocations;
        }

        public async Task CreateAsync
            (

            )
        {

        }
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //Private Methods
    }
}
