using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.Entities;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Company.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
namespace Domain.Features.Branch.Entities
{
    public class DomainBranch : Entity<BranchId>
    {
        //Values
        public UrlSegment? UrlSegment { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }


        //References
        //DomainCompany
        public UserId CompanyId { get; private set; } = null!;
        private DomainCompany _company = null!;
        public DomainCompany Company
        {
            get { return _company; }
            set
            {
                if (_company == null && value != null && value.Id == CompanyId)
                {
                    _company = value;
                    _company.AddBranches([this]);
                }
            }
        }

        //BranchOffer 
        private Dictionary<BranchOfferId, DomainBranchOffer> _branchOffers = [];
        public IReadOnlyDictionary<BranchOfferId, DomainBranchOffer> BranchOffers => _branchOffers;

        //Adress
        public AddressId AddressId { get; private set; } = null!;
        private DomainAddress _address = null!;
        public DomainAddress Address
        {
            get { return _address; }
            set
            {
                if (value.Id != AddressId)
                {
                    throw new AddressException
                        (
                        Messages.Branch_Address_NotSameAddressId,
                        DomainExceptionTypeEnum.AppProblem
                        );
                }
                _address = value;
            }
        }


        //Constructor
        public DomainBranch
            (
            Guid? id,
            Guid companyId,
            Guid addressId,
            string? urlSegment,
            string name,
            string? description,
            IProvider provider
            ) : base(new BranchId(id), provider)
        {
            CompanyId = new UserId(companyId);
            Update
                (
                addressId,
                urlSegment,
                name,
                description
                );
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void AddBranchOffers(IEnumerable<DomainBranchOffer> branchOffers)
        {
            foreach (var branchOffer in branchOffers)
            {
                AddBranchOffer(branchOffer);
            }
        }

        public void Update
            (
            Guid addressId,
            string? urlSegment,
            string name,
            string? description
            )
        {
            AddressId = new AddressId(addressId);
            UrlSegment = (UrlSegment?)urlSegment;
            Name = name;
            Description = description;
        }

        public static
            (
            IEnumerable<DomainBranch> Correct,
            Dictionary<DomainBranch, List<DomainBranch>> Duplicates
            )
            SeparateAndFilterBranches(IEnumerable<DomainBranch> domainBranches)
        {
            var correct = new List<DomainBranch>();
            var correctDictionary = new Dictionary<string, DomainBranch>();
            var duplicates = new Dictionary<DomainBranch, List<DomainBranch>>();
            //Url segments
            foreach (var branch in domainBranches)
            {
                if (branch.UrlSegment == null)
                {
                    correct.Add(branch);
                }
                else
                {
                    var url = branch.UrlSegment.Value;
                    if (!correctDictionary.TryGetValue(url, out var value))
                    {
                        correctDictionary[url] = branch;
                    }
                    else
                    {
                        //If Exist In Dictionary means is Duplicate
                        if (duplicates.TryGetValue(value, out var duplicate))
                        {
                            duplicate.Add(branch);
                        }
                        else
                        {
                            duplicates[value] = new List<DomainBranch>() { branch };
                        }
                    }
                }
            }

            foreach (var item in duplicates)
            {
                if (item.Key.UrlSegment != null)
                {
                    correctDictionary.Remove(item.Key.UrlSegment.Value);
                }
            }

            correct.AddRange(correctDictionary.Values);
            return (correct, duplicates);
        }

        public static
            (
            IEnumerable<DomainBranch> Correct,
            Dictionary<DomainBranch, DomainBranch> Duplicates
            )
            SeparateAndFilterBranchesFromDB(IEnumerable<DomainBranch> databases, IEnumerable<DomainBranch> inputs)
        {
            var inputsDictionary = inputs
                .Where(x => x.UrlSegment != null)
                .ToDictionary(x => x.UrlSegment.Value);

            var correct = new List<DomainBranch>();
            var duplicates = new Dictionary<DomainBranch, DomainBranch>();

            foreach (var item in inputs)
            {
                if (item.UrlSegment == null)
                {
                    correct.Add(item);
                }
                else
                {
                    var url = item.UrlSegment.Value;
                    if (inputsDictionary.TryGetValue(url, out var database))
                    {
                        duplicates[database] = item;
                    }
                    else
                    {
                        correct.Add(item);
                    }
                }
            }

            return (correct, duplicates);
        }
        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
        private void AddBranchOffer(DomainBranchOffer domainBranchOffer)
        {
            if (
                domainBranchOffer.BranchId == Id &&
                !_branchOffers.ContainsKey(domainBranchOffer.Id)
                )
            {
                _branchOffers.Add(domainBranchOffer.Id, domainBranchOffer);
                domainBranchOffer.Branch = this;
            }
        }
    }
}
