using Domain.Features.Url.ValueObjects;

namespace Domain.Features.Url.Repository
{
    public interface IDomainUrlTypeDictionariesRepository
    {
        Dictionary<int, DomainUrlType> GetDomainUrlTypeDictionary();
    }
}
