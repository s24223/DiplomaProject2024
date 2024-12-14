using Application.Features.Addresses.Commands.DTOs;
using Application.Features.Addresses.Commands.Interfaces;
using Application.Shared.DTOs.Response;
using Domain.Shared.Factories;

namespace Application.Features.Addresses.Commands.Services
{
    public class AddressCmdSvc : IAddressCmdSvc
    {
        //Values
        private readonly IAddressCmdRepo _repository;
        private readonly IDomainFactory _domainFactory;


        //Cosntructor
        public AddressCmdSvc
            (
            IAddressCmdRepo repository,
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
        public async Task<ResponseItem<CreateAddressResp>> CreateAsync
            (
            CreateAddressReq dto,
            CancellationToken cancellation
            )
        {
            string kraj = dto.Country.Trim();
            string wojewodztwo = dto.State.Replace("województwo", "").Trim();
            string? powiat = dto.County?.Replace("powiat", "").Trim();
            string? gmina = dto.Municipality?.Replace("gmina", "").Trim();
            string city = dto.City.Trim();
            string? dzielnica = dto.Suburb?.Trim();
            string? street = dto.Street?.Trim();

            var addressId = await _repository.CreateAsync(
                wojewodztwo,
                powiat,
                gmina,
                city,
                dzielnica,
                street,
                dto.Lon,
                dto.Lat,
                dto.Postcode,
                dto.HouseNumber,
                dto.ApartmentNumber,
                cancellation);

            return new ResponseItem<CreateAddressResp>
            {
                Item = new CreateAddressResp
                {
                    AddressId = addressId,
                }
            };
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
