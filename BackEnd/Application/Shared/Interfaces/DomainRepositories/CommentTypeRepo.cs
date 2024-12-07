using Application.Databases.Relational;
using Domain.Features.Comment.Exceptions.ValueObjects;
using Domain.Features.Comment.Reposoitories;
using Domain.Features.Comment.ValueObjects.CommentTypePart;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Shared.Interfaces.DomainRepositories
{
    public class CommentTypeRepo : ICommentTypeRepo
    {
        //Values
        private readonly DiplomaProjectContext _context;
        private readonly IMemoryCache _cache;
        private readonly string CacheKeyCommentTypes = "CommentTypes";

        //Magic Numbers
        private readonly int _absoluteExpirationHours = 1;
        private readonly int _slidingExpirationFromMinutes = 5;


        //Cosntructor
        public CommentTypeRepo
            (
            DiplomaProjectContext context,
            IMemoryCache cache
            )
        {
            _cache = cache;
            _context = context;
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public Dictionary<int, DomainCommentType> GetDictionary()
        {
            return GetDictionaryMethod();
        }

        public DomainCommentType GetValue(int id)
        {
            if (GetDictionaryMethod().TryGetValue(id, out var item))
            {
                return item;
            }
            throw new CommentTypeException(
                $"{Messages.CommentType_Qwery_IdItemNotFound}: {id}",
                Domain.Shared.Templates.Exceptions.DomainExceptionTypeEnum.AppProblem);
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        private Dictionary<int, DomainCommentType> GetDictionaryMethod()
        {
            if (!_cache.TryGetValue(
                    CacheKeyCommentTypes,
                    out Dictionary<int, DomainCommentType>? dictionary) ||
                dictionary == null
                )
            {
                dictionary = _context.CommentTypes.Select(x => new DomainCommentType(
                    x.Id,
                    x.Name,
                    x.Description
                    ))
                    .ToDictionary(x => x.Id);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_absoluteExpirationHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_slidingExpirationFromMinutes)
                };

                _cache.Set(CacheKeyCommentTypes, dictionary, cacheOptions);
            }
            return dictionary;
        }
    }
}
