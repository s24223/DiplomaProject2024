using Domain.Features.Address.Entities;
using Domain.Features.Address.Exceptions.Entities;
using Domain.Features.Address.ValueObjects.Identificators;
using Domain.Features.Characteristic.Entities;
using Domain.Features.Characteristic.Repositories;
using Domain.Features.Characteristic.ValueObjects.Identificators;
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
        private readonly ICharacteristicQueryRepository _characteristicRepository;

        //Values
        public UrlSegment? UrlSegment { get; private set; }
        public DateOnly Created { get; private set; }
        public Email ContactEmail { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string Surname { get; private set; } = null!;
        public DateOnly? BirthDate { get; private set; } = null;
        public PhoneNumber? ContactPhoneNum { get; private set; }
        public string? Description { get; private set; } = null;
        public DatabaseBool IsStudent { get; private set; } = null!;
        public DatabaseBool IsPublicProfile { get; private set; } = null!;
        public int? Age { get; private set; } = null;


        //References
        //User
        private DomainUser _user = null!;
        //Address
        public AddressId? AddressId { get; private set; } = null;
        private DomainAddress _address = null!;
        //Recrutments
        private Dictionary<RecrutmentId, DomainRecruitment> _recrutments = new();
        public IReadOnlyDictionary<RecrutmentId, DomainRecruitment> Recrutments => _recrutments;
        //Characteristic
        private Dictionary<CharacteristicId, (DomainCharacteristic, DomainQuality?)>
            _characteristics = [];
        public IReadOnlyDictionary<CharacteristicId, (DomainCharacteristic, DomainQuality?)>
            Characteristics => _characteristics;


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
            ICharacteristicQueryRepository characteristicRepository,
            IProvider provider
            ) : base(new UserId(id), provider)
        {
            _characteristicRepository = characteristicRepository;

            SetData(urlSegment, contactEmail, name, surname, birthDate, contactPhoneNum, description,
                isStudent, isPublicProfile, addressId);
            Created = created ?? _provider.TimeProvider().GetDateOnlyToday();
        }


        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Public Methods
        //Default Setters
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

        //Custom setters
        public void SetRecrutment(DomainRecruitment domainRecrutment)
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
            SetData(urlSegment, contactEmail, name, surname, birthDate, contactPhoneNum, description,
                isStudent, isPublicProfile, addressId);

            //ThrowExceptionIfIsNotValid();
        }

        public void SetCharacteristics(IEnumerable<(CharacteristicId, QualityId?)> values)
        {
            _characteristics.Clear();

            var dictionary = _characteristicRepository.GetCollocations(values);
            foreach (var pair in dictionary)
            {
                _characteristics[pair.Key.CharacteristicId] = pair.Value;
            }
        }

        //=================================================================================================
        //=================================================================================================
        //=================================================================================================
        //Private Methods
        private void SetData
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
        }

        private void SetData
            (
            string? urlSegment,
            string contactEmail,
            string name,
            string surname,
            DateOnly? birthDate,
            string? contactPhoneNum,
            string? description,
            string isStudent,
            string isPublicProfile,
            Guid? addressId
            )
        {
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
            Description = description;
            AddressId = addressId == null ?
                null : new AddressId(addressId.Value);
            if (birthDate.HasValue)
            {
                BirthDate = birthDate;
                Age = _provider.TimeProvider().YearsDifference(birthDate.Value);
            }
        }

        /*
                private void ThrowExceptionIfIsNotValid()
                {
                    if (
                        BirthDate is not null &&
                        BirthDate > _provider.TimeProvider().GetDateOnlyToday()
                        )
                    {
                        throw new PersonException(Messages.Person_BirthDate_InFuture);
                    }
                }*/
    }
}
