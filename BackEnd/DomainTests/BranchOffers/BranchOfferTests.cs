using Domain.Shared.Factories;

namespace DomainTests.BranchOffers
{
    public class BranchOfferTests
    {
        private readonly IDomainFactory _domainFactory;

        public BranchOfferTests
            (
            IDomainFactory domainFactory
            )
        {
            _domainFactory = domainFactory;
        }


        public void x()
        {
            var x1 = _domainFactory.CreateDomainBranchOffer
                (
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.Now,
                null,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now)
                );

            Assert.Equal(x1, x1);
        }
    }
}
