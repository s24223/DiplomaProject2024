using Application.Features.Addresses.Queries.DTOs;
using Application.Features.Addresses.Queries.Interfaces;
using Application.Shared.DTOs.Features.Addresses;
using Application.Shared.DTOs.Response;
using Domain.Features.Address.ValueObjects.Identificators;

namespace Application.Features.Addresses.Queries.Services
{
    public class AddressQueryService : IAddressQueryService
    {
        //Values
        private readonly IAddressQueryRepo _repository;


        //Cosntructor
        public AddressQueryService
            (
            IAddressQueryRepo repository
            )
        {
            _repository = repository;
        }



        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        //DQL
        public async Task<ResponseItems<CollocationResponseDto>> GetCollocationsAsync
            (
            string divisionName,
            string streetName,
            CancellationToken cancellation
            )
        {
            var collocations = await _repository.GetCollocationsAsync(divisionName, streetName, cancellation);
            return new ResponseItems<CollocationResponseDto>
            {
                Items = collocations.ToList()
            };
        }

        public async Task<ResponseItem<AddressResponseDto>> GetAddressAsync
            (
            Guid id,
            CancellationToken cancellation
            )
        {
            var address = await _repository.GetAddressAsync(new AddressId(id), cancellation);
            return new ResponseItem<AddressResponseDto>
            {
                Item = new AddressResponseDto(address),
            };
        }

        public async Task<ResponseItems<DivisionStreetsResponseDto>> GetDivisionsDownAsync
            (
            int? id,
            CancellationToken cancellation
            )
        {
            DivisionId? divisionId = id.HasValue ? new DivisionId(id.Value) : null;
            var items = await _repository.GetDivisionsDownVerticalAsync(divisionId, cancellation);

            return new ResponseItems<DivisionStreetsResponseDto>
            {
                Items = items.ToList(),
            };
        }

        public async Task<IEnumerable<DivisionUpResp>> GetDivisionsDownHorizontalAsync(
            int? divisionId,
            CancellationToken cancellation
            )
        {
            return await _repository.GetDivisionsDownHorizontalAsync(divisionId, cancellation);
        }

        public async Task<IEnumerable<StreetResponseDto>> GetStreetsAsync
            (
            int divisionId,
            CancellationToken cancellation
            )
        {
            return await _repository.GetStreetsAsync(divisionId, cancellation);
        }
        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
