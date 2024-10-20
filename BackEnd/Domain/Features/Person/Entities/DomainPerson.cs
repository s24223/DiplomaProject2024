using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Person.Exceptions.Entities;
using Domain.Features.Person.ValueObjects;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Features.User.Entities;
using Domain.Features.User.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.Templates.Exceptions;
using Domain.Shared.ValueObjects;
namespace Domain.Features.Person.Entities
{
    public class DomainPerson : Entity<UserId>
    {
        //Values
        public UrlSegment? UrlSegment { get; private set; }
        public DateOnly Created { get; private set; }
        public Email ContactEmail { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Surname { get; private set; } = null!;
        public DateOnly? BirthDate { get; private set; }
        public PhoneNumber? ContactPhoneNum { get; private set; }
        public string? Description { get; private set; }
        public DatabaseBool IsStudent { get; private set; }
        public DatabaseBool IsPublicProfile { get; private set; }


        //References
        //User
        private DomainUser _user = null!;
        public DomainUser User
        {
            get { return _user; }
            set
            {
                if (_user == null && value != null && value.Id == Id)
                {
                    _user = value;
                    _user.Person = this;
                }
            }
        }
        //Recrutment
        private Dictionary<RecrutmentId, DomainRecruitment> _recrutments = new();
        public IReadOnlyDictionary<RecrutmentId, DomainRecruitment> Recrutments => _recrutments;

        //Address
        public AddressId? AddressId { get; private set; } = null;
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
                        Messages.Person_Address_NotSameAddressId,
                        DomainExceptionTypeEnum.AppProblem
                        );
                }
                _address = value;
            }
        }


        //Constructor
        public DomainPerson
            (
            Guid id,
            string? urlSegment,
            DateOnly? created,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            string isStudent,
            string isPublicProfile,
            Guid? addressId,
            IProvider provider
            ) : base(new UserId(id), provider)
        {

            //Values with exeptions
            ContactEmail = new Email(value: contactEmail);
            IsStudent = new DatabaseBool(isStudent);
            IsPublicProfile = new DatabaseBool(isPublicProfile);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);
            ContactPhoneNum = string.IsNullOrWhiteSpace(contactPhoneNum) ?
                null : new PhoneNumber(contactPhoneNum);

            //Values with no exeptions
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Description = description;
            AddressId = addressId == null ?
                null : new AddressId(addressId.Value);
            Created = created ?? _provider.TimeProvider().GetDateOnlyToday();


            //ThrowExceptionIfIsNotValid();
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        public void AddRecrutment(DomainRecruitment domainRecrutment)
        {
            if (domainRecrutment.PersonId == Id && !_recrutments.ContainsKey(domainRecrutment.Id))
            {
                _recrutments.Add(domainRecrutment.Id, domainRecrutment);
                domainRecrutment.Person = this;
            }
        }

        public void Update
            (
            string? urlSegment,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            bool isStudent,
            bool isPublicProfile,
            Guid? addressId
            )
        {
            //Values with exeptions
            ContactEmail = new Email(value: contactEmail);
            IsStudent = new DatabaseBool(isStudent);
            IsPublicProfile = new DatabaseBool(isPublicProfile);
            UrlSegment = string.IsNullOrWhiteSpace(urlSegment) ?
                null : new UrlSegment(urlSegment);
            ContactPhoneNum = string.IsNullOrWhiteSpace(contactPhoneNum) ?
                null : new PhoneNumber(contactPhoneNum);

            //Values with no exeptions
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Description = description;
            AddressId = addressId == null ?
                null : new AddressId(addressId.Value);

            ThrowExceptionIfIsNotValid();
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
        private void ThrowExceptionIfIsNotValid()
        {
            if (
                BirthDate is not null &&
                BirthDate > _provider.TimeProvider().GetDateOnlyToday()
                )
            {
                throw new PersonException(Messages.Person_BirthDate_InFuture);
            }
        }
    }
}
