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
        //Pochodne
        public Duration HowLongExist { get; private set; }



        //Refrences
        //DomainUser
        private DomainUser _user = null!;
        //DomainBranch
        private Dictionary<BranchId, DomainBranch> _branches = [];
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
            Regon = new Regon(regon);
            UpdateData
                (
                urlSegment,
                contactEmail,
                name,
                description
                );

            Created = created ?? provider.TimeProvider().GetDateOnlyToday();
            HowLongExist = new Duration(Created, _provider.TimeProvider().GetDateOnlyToday());
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
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

        public void AddBranches(IEnumerable<DomainBranch> branches)
        {
            var newBranchesDictionary = branches
                .Where(x => x.CompanyId == Id && !_branches.ContainsKey(x.Id))
                .ToDictionary(x => x.Id, x => x);

            foreach (var branch in newBranchesDictionary)
            {
                _branches.Add(branch.Key, branch.Value);
                branch.Value.Company = this;
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
            ContactEmail = new Email(contactEmail);
            UrlSegment = (UrlSegment?)urlSegment;
            Name = name;
            Description = description;
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
