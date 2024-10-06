using Domain.Features.Offer.Exceptions;
using Domain.Features.Offer.ValueObjects;

namespace DomainTests
{
    public class UnitTest1
    {
        [InlineData(1.01)]
        [InlineData(2.0)]
        [InlineData(2)]
        [Theory]
        public void Test1(decimal value)
        {
            //arrange
            //Act
            var result = new Money(value);

            //Assert
            Assert.Equal(value, result.Value);
        }

        [InlineData(1.001)]
        [Theory]
        public void Test2(decimal value)
        {
            //arrange
            //Act
            //Assert
            Assert.Throws<MoneyException>(() => new Money(value));
        }

        [Fact]
        public void Test3()
        {
            var value = (decimal)2.3;
            var money1 = new Money(value);
            var money2 = new Money(value);
            //arrange
            //Act
            var result = money1 == money2;
            //Assert
            Assert.True(result);
        }
    }
}