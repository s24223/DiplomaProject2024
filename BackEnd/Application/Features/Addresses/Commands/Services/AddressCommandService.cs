using Application.Features.Addresses.Commands.DTOs.Create;
using Application.Features.Addresses.Commands.DTOs.Update;
using Application.Features.Addresses.Commands.Interfaces;
using Application.Shared.DTOs.Response;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Shared.Factories;

namespace Application.Features.Addresses.Commands.Services
{
    public class AddressCommandService : IAddressCommandService
    {
        //Values
        private readonly IAddressCommandRepository _repository;
        private readonly IDomainFactory _domainFactory;


        //Cosntructor
        public AddressCommandService
            (
            IAddressCommandRepository repository,
            IDomainFactory domainFactory
            )
        {
            _repository = repository;
            _domainFactory = domainFactory;

        }



        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        //DML
        public async Task<ResponseItem<CreateAddressResponseDto>> CreateAsync
            (
            CreateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            var adress = _domainFactory.CreateDomainAddress
                (
                dto.DivisionId,
                dto.StreetId,
                dto.BuildingNumber,
                dto.ApartmentNumber,
                dto.ZipCode
                );
            var addressId = await _repository.CreateAsync(adress, cancellation);
            return new ResponseItem<CreateAddressResponseDto>
            {
                Item = new CreateAddressResponseDto
                {
                    AddressId = addressId,
                }
            };
        }

        public async Task<Response> UpdateAsync
            (
            Guid id,
            UpdateAddressRequestDto dto,
            CancellationToken cancellation
            )
        {
            var address = await _repository.GetAddressAsync(new AddressId(id), cancellation);
            address.SetZipCode(dto.ZipCode);
            await _repository.UpdateAsync(address, cancellation);
            return new Response { };
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
