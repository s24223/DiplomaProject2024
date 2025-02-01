using Domain.Features.Branch.Entities;
using Domain.Features.Branch.ValueObjects.Identificators;
using Domain.Features.BranchOffer.ValueObjects.Identificators;
using Domain.Features.Offer.Entities;
using Domain.Features.Offer.ValueObjects.Identificators;
using Domain.Features.Recruitment.Entities;
using Domain.Features.Recruitment.ValueObjects.Identificators;
using Domain.Shared.Providers;
using Domain.Shared.Templates.Entities;
using Domain.Shared.ValueObjects;

namespace Domain.Features.BranchOffer.Entities
{
    public class DomainBranchOffer : Entity<BranchOfferId>
    {
        //Values
        public DateTime Created { get; private set; }
        public DateTime PublishStart { get; private set; }
        public DateTime? PublishEnd { get; private set; }
        public DateOnly? WorkStart { get; private set; }
        public DateOnly? WorkEnd { get; private set; }
        public DateTime LastUpdate { get; private set; }
        //Pochodne
        public Duration? WorkDuration { get; private set; }


        //References
        //DomainBranch
        public BranchId BranchId { get; private set; }
        private DomainBranch _branch = null!;
        public DomainBranch Branch
        {
            get { return _branch; }
            set
            {
                if (_branch == null && value != null && value.Id == BranchId)
                {
                    _branch = value;
                    _branch.AddBranchOffers([this]);
                }
            }
        }

        //DomainOffer
        public OfferId OfferId { get; private set; }
        private DomainOffer _offer = null!;
        public DomainOffer Offer
        {
            get { return _offer; }
            set
            {
                if (_offer == null && value != null && value.Id == OfferId)
                {
                    _offer = value;
                    _offer.SetBranchOffers([this]);
                }
            }
        }

        //DomainRecrutment
        private Dictionary<RecrutmentId, DomainRecruitment> _recrutments = [];
        public IReadOnlyDictionary<RecrutmentId, DomainRecruitment> Recrutments => _recrutments;


        //Constructor
        public DomainBranchOffer
            (
            Guid? id,
            Guid branchId,
            Guid offerId,
            DateTime? created,
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd,
            DateTime? lastUpdate,
            IProvider provider
            ) : base(new BranchOfferId(id), provider)
        {
            LastUpdate = lastUpdate ?? _provider.TimeProvider().GetDateTimeNow();
            Created = created ?? _provider.TimeProvider().GetDateTimeNow();

            BranchId = new BranchId(branchId);
            OfferId = new OfferId(offerId);

            SetData(publishStart, publishEnd, workStart, workEnd);
        }


        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Public Methods
        public void AddRecrutments(IEnumerable<DomainRecruitment> recrutments)
        {
            foreach (var recrutment in recrutments)
            {
                AddRecrutment(recrutment);
            }
        }

        public void Update
            (
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd
            )
        {
            //Publish start in Past publishStart = PublishStart
            SetData(publishStart, publishEnd, workStart, workEnd);
            LastUpdate = _provider.TimeProvider().GetDateTimeNow();
        }

        /*
                public static
                    (
                    IEnumerable<(DomainBranchOffer Core, DomainBranchOffer Duplicate)> Duplicates,
                    IEnumerable<DomainBranchOffer> Correct
                    )
                    ReturnDuplicatesAndCorrectValues(IEnumerable<DomainBranchOffer> branchOffers)
                {
                    if (branchOffers.Count() < 2)
                    {
                        return ([], branchOffers);
                    }

                    var groups = branchOffers.GroupBy(x => new { x.BranchId, x.OfferId });
                    var correctEndList = new List<DomainBranchOffer>();
                    var duplicatesEndList = new List<(DomainBranchOffer Core, DomainBranchOffer Duplicate)>();

                    if (groups.Count() == branchOffers.Count())
                    {
                        return (duplicatesEndList, branchOffers);
                    }

                    foreach (var group in groups)
                    {
                        var groupList = group.ToList();
                        var correct = new List<DomainBranchOffer>();
                        var duplicates = new List<(DomainBranchOffer Core, DomainBranchOffer Duplicate)>();

                        //Wyszukaj najwczesniejszy nielimitowany PublishEnd
                        var minUnlimited = groupList.Where(x => !x.PublishEnd.HasValue).OrderBy(x => x.PublishStart).FirstOrDefault();
                        if (minUnlimited != null)
                        {
                            correct.Add(minUnlimited);
                            groupList.Remove(minUnlimited);

                            var removeRange = groupList.Where(x => x.PublishStart >= minUnlimited.PublishStart).ToList();
                            foreach (var remove in removeRange)
                            {
                                duplicates.Add((minUnlimited, remove));
                            }
                            groupList.RemoveAll(x => removeRange.Contains(x));
                        }

                        //Jesli lista niepiusta
                        if (groupList.Any())
                        {
                            //Działaj z tymi które maja watosci PublishEnd
                            groupList = groupList
                                .Where(x => x.PublishEnd.HasValue)
                                .OrderBy(x => x.PublishEnd)
                                .ToList();

                            do
                            {
                                var last = groupList.Last();
                                groupList.Remove(last);
                                correct.Add(last);

                                var listAfter = groupList
                                    .Where(x => x.PublishEnd >= last.PublishStart || x.PublishStart >= last.PublishStart)
                                    .ToList();

                                foreach (var removeItem in listAfter)
                                {
                                    duplicates.Add((last, removeItem));
                                }
                                groupList.RemoveAll(x => listAfter.Contains(x));

                            } while (groupList.Any());
                            correctEndList.AddRange(correct);
                            duplicatesEndList = duplicates;
                        }
                    }
                    return (duplicatesEndList, correctEndList);
                }
        */

        public static
            (
            IEnumerable<DomainBranchOffer> Correct,
            IEnumerable<DomainBranchOffer> WithoutDuration,
            Dictionary<DomainBranchOffer, List<DomainBranchOffer>> Conflicts
            )
            SeparateAndFilterBranchOffers(IEnumerable<DomainBranchOffer> values)
        {
            if (values.Count() == 1)
            {
                return (values, [], []);
            }

            var correctList = new List<DomainBranchOffer>();
            var valuesWithoutDuration = new List<DomainBranchOffer>();
            var conflictDictionary = new Dictionary<DomainBranchOffer, List<DomainBranchOffer>>();

            //Ignore values with PublishStart == x.PublishEnd, beacause duration them == 0
            var listWithDuration = new List<DomainBranchOffer>();
            foreach (var item in values)
            {
                if (item.PublishEnd.HasValue && item.PublishStart == item.PublishEnd.Value)
                {
                    valuesWithoutDuration.Add(item);
                }
                else
                {
                    listWithDuration.Add(item);
                }
            }

            //Grupownaie
            var groupsDictionary = listWithDuration
                .GroupBy(val => (val.OfferId, val.BranchId))
                .ToDictionary(g => g.Key, g => g.ToList());

            //Working With Groups
            foreach (var group in groupsDictionary)
            {
                //If only 1 value hs not any Conflicts
                if (group.Value.Count() == 1)
                {
                    correctList.Add(group.Value.First());
                }

                //Work with values which has value
                var dictionary = new SortedDictionary<DateTime, DomainBranchOffer>();

                //Remove Duplicates
                foreach (var item in group.Value)
                {
                    //Key item.PublishStart for 'dictionary'
                    if (!dictionary.TryGetValue(item.PublishStart, out var dictionaryValue))
                    {
                        dictionary[item.PublishStart] = item;
                    }
                    else
                    {
                        //Key dictionaryValue for 'conflictDictionary'
                        if (!conflictDictionary.TryGetValue(dictionaryValue, out var conflict))
                        {
                            conflictDictionary[dictionaryValue] =
                                new List<DomainBranchOffer>() { item };
                        }
                        else
                        {
                            conflict.Add(item);
                        }
                    }
                }

                //If only 1 value hs not any Conflicts from separate 'dictionary'
                if (dictionary.Count() == 1)
                {
                    correctList.Add(dictionary.First().Value);
                }

                //Work with values with Duration and without Duplicates
                while (dictionary.Count > 0)
                {
                    var first = dictionary.First();
                    var publishEnd = first.Value.PublishEnd;

                    dictionary.Remove(first.Key);
                    correctList.Add(first.Value);


                    //Work with conflicts
                    var conflictList = new List<DomainBranchOffer>();

                    //if first have infity time
                    if (publishEnd == null)
                    {
                        conflictList = dictionary.Values.ToList();
                        dictionary.Clear();
                    }
                    else
                    {
                        var conflicts = dictionary.Where(x => x.Key <= publishEnd).ToDictionary();
                        foreach (var c in conflicts)
                        {
                            dictionary.Remove(c.Key);
                        }
                        conflictList.AddRange(dictionary.Values);
                    }

                    //Set conflicts
                    if (conflictList.Any())
                    {
                        if (!conflictDictionary.TryGetValue(first.Value, out var conflict))
                        {
                            conflictDictionary[first.Value] = conflictList;
                        }
                        else
                        {
                            conflict.AddRange(conflictList);
                        }
                    }
                }
            }
            return (correctList, valuesWithoutDuration, conflictDictionary);
        }

        //===================================================================================================
        //===================================================================================================
        //===================================================================================================
        //Private Methods
        private void AddRecrutment(DomainRecruitment domainRecrutment)
        {
            if (
                domainRecrutment.BranchOfferId == Id &&
                !_recrutments.ContainsKey(domainRecrutment.Id)
                )
            {
                _recrutments.Add(domainRecrutment.Id, domainRecrutment);
                domainRecrutment.BranchOffer = this;
            }
        }

        private void SetData
            (
            DateTime publishStart,
            DateTime? publishEnd,
            DateOnly? workStart,
            DateOnly? workEnd
            )
        {
            PublishStart = publishStart;
            PublishEnd = publishEnd;
            WorkStart = workStart;
            WorkEnd = workEnd;

            WorkDuration = (WorkStart is null || WorkEnd is null) ?
                null : new Duration(WorkStart.Value, WorkEnd.Value);

            //ThrowExceptionIfIsNotValid();
        }

        /*
                private void ThrowExceptionIfIsNotValid()
                {
                    if (
                        PublishEnd is not null &&
                        PublishEnd < PublishStart
                        )
                    {
                        //Context: Data konca publikacji nie może byc mniejsza a niz data obecna
                        throw new BranchOfferException(Messages.BranchOffer_PublishEnd_Invalid);
                    }
                    if (
                        PublishEnd is not null &&
                        WorkStart is not null &&
                        _provider.TimeProvider().ToDateTime(WorkStart.Value) < PublishEnd
                        )
                    {
                        //Context: Data Początku pracy powinna być najwczesniij kolejnego dnia po ukonczeniu rekrutacji
                        throw new BranchOfferException(Messages.BranchOffer_WorkStart_Invalid);
                    }
                    if (
                        WorkStart is not null &&
                        WorkEnd is not null &&
                        WorkEnd < WorkStart
                        )
                    {
                        //Context: Data pocztku pracy nie może byc wieksza od konca
                        throw new BranchOfferException(Messages.BranchOffer_WorkEnd_Invalid);
                    }
                }*/
    }
}
