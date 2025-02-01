using Domain.Features.Url.Repository;
using Domain.Features.Url.ValueObjects;

namespace DomainTests.Fakes
{
    public class DomainUrlFake : IDomainUrlTypeDictionariesRepository
    {
        public Dictionary<int, DomainUrlType> GetDomainUrlTypeDictionary()
        {
            return new Dictionary<int, DomainUrlType>()
            {
                {1, new DomainUrlType(1, "First", "Description") },
                {2, new DomainUrlType(2, "Hand", "Description") },
                {3, new DomainUrlType(3, "Hand", "Description") },
            };
        }
    }
}
