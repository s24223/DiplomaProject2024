using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.Company.ValueObjects;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;
namespace Domain.Features.Company.Entities
{
    public class DomainCompany : Entity<UserId>
    {
        //Values
        public DateOnly Created { get; private set; }
        public UrlSegment? UrlSegment { get; private set; } = null;
        public Email ContactEmail { get; private set; } = null!;
        public Regon Regon { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }


        //Refrences
        //DomainUser
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == Id)
                {
                    _user = value;
                    _user.Company = this;
                }
            }
        }
        //DomainBranch
        private Dictionary<BranchId, DomainBranch> _branches = new();
        public IReadOnlyDictionary<BranchId, DomainBranch> Branches => _branches;


        //Constructor
        public DomainCompany
            (
            Guid id,
            string? urlSegment,
            string contactEmail,
            string name,
            string regon,
            string? description,
            DateOnly? created,
            IProvider provider
            ) : base(new UserId(id), provider)
        {
            //Values with exeptions
            Regon = new Regon(regon);
            ContactEmail = new Email(contactEmail);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);

            //Values with no exeptions
            Name = name;
            Description = description;
            Created = created ?? provider.TimeProvider().GetDateOnlyToday();
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        public void AddBranch(DomainBranch domainBranch)
        {
            if (domainBranch.CompanyId == Id && !_branches.ContainsKey(domainBranch.Id))
            {
                _branches.Add(domainBranch.Id, domainBranch);
                domainBranch.Company = this;
            }
        }

        public void UpdateData
            (
            string? urlSegment,
            string contactEmail,
            string name,
            string? description
            )
        {
            //Values with exeptions
            ContactEmail = new Email(contactEmail);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);

            //Values with no exeptions
            Name = name;
            Description = description;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
