using Domain.Entities.UserPart;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;
namespace Domain.Entities.CompanyPart
{
    public class DomainCompany : Entity<UserId>
    {
        //Values
        public DateOnly CreateDate { get; private set; }
        public SegementUrl? UrlSegment { get; set; } = null;
        public Email ContactEmail { get; set; } = null!;
        public Regon Regon { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        //Refrences
        public DomainUser User { get; set; } = null!;

        //Constructor
        public DomainCompany
            (
            Guid id,
            string? urlSegment,
            string contactEmail,
            string name,
            string regon,
            string? description,
            DateOnly? createDate,
            IDomainProvider provider
            ) : base(new UserId(id), provider)
        {
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ? null : new SegementUrl(urlSegment);
            CreateDate = createDate != null ? createDate.Value : _provider.GetTimeProvider().GetDateOnlyToday();
            ContactEmail = new Email(contactEmail);
            Name = name;
            Regon = new Regon(regon);
            Description = description;
        }
    }
}
