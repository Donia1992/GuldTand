using Guldtand.Domain.Helpers;
using Xunit;

namespace Guldtand.UnitTests
{
    public class PIDValidationTests
    {
        [Theory]
        [InlineData("641020", true)]
        [InlineData("641320", false)]
        [InlineData("641032", false)]
        [InlineData("640020", false)]
        [InlineData("641000", false)]
        [InlineData("a641020", false)]
        public void DateTimeProvider_IsValidDate_OnlyReturnsTrueOnValidDate(string date, bool expectedResult)
        {
            //Arrange
            var sut = new DateTimeProvider();

            //Act
            var result = sut.IsValidDate(date);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("6410207655", true)]
        [InlineData("9302169066", true)]
        [InlineData("6410207654", false)]
        [InlineData("9302169067", false)]
        [InlineData("641020765", false)]
        [InlineData("64102076555", false)]
        public void PIDValidator_Validate_OnlyReturnsTrueOnValidPID(string pid, bool expectedResult)
        {
            //Arrange
            var sut = new PIDValidator();

            //Act
            var result = sut.Validate(pid);

            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
