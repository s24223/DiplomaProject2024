using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.CompanyPart
{
    public class DomainOffer : Entity<OfferId>
    {
        //Values
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Money? MinSalary { get; set; }
        public Money? MaxSalary { get; set; }
        public bool? IsNegotiatedSalary { get; set; }
        public bool ForStudents { get; set; }

        //References

        //Constructor
        public DomainOffer
            (
            Guid? id,
            string name,
            string description,
            decimal? minSalary,
            decimal? maxSalary,
            string? isNegotiatedSalary,
            string forStudents,
            IDomainProvider provider
            ) : base(new OfferId(id), provider)
        {
            Name = name;
            Description = description;
            MinSalary = (minSalary == null) ? null : new Money(minSalary.Value);
            MaxSalary = (maxSalary == null) ? null : new Money(maxSalary.Value);
            IsNegotiatedSalary = (string.IsNullOrWhiteSpace(isNegotiatedSalary)) ?
                null : (isNegotiatedSalary.ToLower() == "y");
            ForStudents = (forStudents.ToLower() == "y");
        }

    }
}
