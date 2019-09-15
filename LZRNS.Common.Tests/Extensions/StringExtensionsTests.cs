using Xunit;

namespace LZRNS.Common.Extensions.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("GAME1", 1)]
        [InlineData("GAME-10", 10)]
        [InlineData("--12MyGame 15", 12)]
        public void ExtractNumber_StringContainsNumber_ReturnsNumber(string input, int expectedResult)
        {
            int actualResult = input.ExtractNumber();

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("GAME", -1)]
        public void ExtractNumber_StringDoesNotContainNumber_ReturnsNumber(string input, int expectedResult)
        {
            int actualResult = input.ExtractNumber();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}