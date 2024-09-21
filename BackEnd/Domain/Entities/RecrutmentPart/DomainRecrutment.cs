using Domain.Entities.CompanyPart;
using Domain.Entities.PersonPart;
using Domain.Providers;
using Domain.Templates.Entities;
using Domain.ValueObjects.EntityIdentificators;

namespace Domain.Entities.RecrutmentPart
{
    public class DomainRecrutment : Entity<RecrutmentId>
    {
        //Values
        public DateTime ApplicationDate { get; private set; }
        public string? PersonMessage { get; set; }
        public string? CompanyResponse { get; set; }
        public bool? AcceptedRejected { get; set; }


        //References
        public DomainPerson Person { get; set; } = null!;
        public DomainBranchOffer BranchOffer { get; set; } = null!;
        public DomainIntership? Intership { get; set; } = null;


        //Cosntructor
        public DomainRecrutment
            (
            Guid personId,
            Guid branchId,
            Guid offerId,
            DateTime created,
            DateTime? applicationDate,
            string? personMessage,
            string? companyResponse,
            string? acceptedRejected,
        IDomainProvider provider
            ) : base(new RecrutmentId
                (
                new BranchOfferId(
                    new BranchId(branchId),
                    new OfferId(offerId),
                    created
                    ),
                new UserId(personId)
                ), provider)
        {

            PersonMessage = personMessage;
            CompanyResponse = companyResponse;

            ApplicationDate = (applicationDate != null) ?
                applicationDate.Value : _provider.GetTimeProvider().GetDateTimeNow();

            AcceptedRejected = (string.IsNullOrWhiteSpace(acceptedRejected)) ?
                null : (acceptedRejected.ToLower() == "y");
        }
    }
}
