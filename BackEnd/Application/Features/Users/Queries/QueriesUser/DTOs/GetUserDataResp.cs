using Application.Shared.DTOs.Features.Characteristics.Responses;
using Application.Shared.DTOs.Features.Companies.Responses;
using Application.Shared.DTOs.Features.Persons;
using Domain.Features.Characteristic.Entities;
using Domain.Features.User.Entities;

namespace Application.Features.Users.Queries.QueriesUser.DTOs
{
    public class GetUserDataResp
    {
        //Values
        public Guid UserId { get; set; }
        public DateTime? LastLoginIn { get; set; } = null;
        public DateTime LastPasswordUpdate { get; set; }
        public PersonResponseDto? Person { get; set; } = null;
        public CompanyResponseDto? Company { get; set; } = null;
        public int? BranchCount { get; set; } = null;
        public int? ActiveOffersCount { get; set; } = null;
        public IEnumerable<CharAndCharTypeResp>? CompanyCharacteristics { get; set; } = null;


        //Constructor
        public GetUserDataResp(
            DomainUser user,
            int branchCount,
            int activeOffersCount,
            IEnumerable<DomainCharacteristic> characteristics
            )
        {
            UserId = user.Id.Value;
            LastLoginIn = user.LastLoginIn;
            LastPasswordUpdate = user.LastPasswordUpdate;

            if (user.Person != null)
            {
                Person = new PersonResponseDto(user.Person);
            }
            if (user.Company != null)
            {
                Company = new CompanyResponseDto(user.Company);
                BranchCount = branchCount;
                ActiveOffersCount = activeOffersCount;
                CompanyCharacteristics = characteristics
                    .Select(x => new CharAndCharTypeResp(x));
            }
        }
    }
}
