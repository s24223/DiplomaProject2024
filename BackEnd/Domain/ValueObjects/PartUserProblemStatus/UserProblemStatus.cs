using Domain.Exceptions.UserExceptions.ValueObjectsExceptions;

namespace Domain.ValueObjects.PartUserProblemStatus
{
    public record UserProblemStatus
    {
        private static Dictionary<string, UserProblemStatus> _statuses = new();


        //Values
        public string Code { get; private set; }
        public UserProblemStatusEnum Name { get; private set; }


        //Cosntructors
        public UserProblemStatus(string? value)
        {
            if (value == null)
            {
                value = "c";
            }

            value = value.ToLower();

            if (!_statuses.TryGetValue(value, out var item))
            {
                throw new UserProblemStatusException(Messages.NotExistUserProblemStatus);
            }
            Code = item.Code;
            Name = item.Name;
        }

        static UserProblemStatus()
        {
            var list = new List<UserProblemStatus>();
            list.Add(new UserProblemStatus("c", UserProblemStatusEnum.Created));
            list.Add(new UserProblemStatus("v", UserProblemStatusEnum.Verifing));
            list.Add(new UserProblemStatus("d", UserProblemStatusEnum.Done));
            list.Add(new UserProblemStatus("a", UserProblemStatusEnum.Annulled));

            foreach (var item in list)
            {
                _statuses.Add(item.Code, item);
            }
        }

        private UserProblemStatus(string code, UserProblemStatusEnum name)
        {
            Code = code;
            Name = name;
        }


        //Methods
        public static IReadOnlyDictionary<string, UserProblemStatus> GetStatuses() => _statuses;
    }
}
