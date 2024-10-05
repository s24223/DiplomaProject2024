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
        public UrlSegment? UrlSegment { get; set; } = null;
        public Email ContactEmail { get; set; } = null!;
        public Regon Regon { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }


        //Refrences
        //DomainUser
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == this.Id)
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
            DateOnly? createDate,
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
            CreateDate = createDate != null ?
                createDate.Value : _provider.TimeProvider().GetDateOnlyToday();
        }


        //Methods
        public void AddBranch(DomainBranch domainBranch)
        {
            if (domainBranch.CompanyId == this.Id && !_branches.ContainsKey(domainBranch.Id))
            {
                _branches.Add(domainBranch.Id, domainBranch);
                domainBranch.Company = this;
            }
        }
    }
}
