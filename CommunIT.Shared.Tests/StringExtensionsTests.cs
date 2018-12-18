using CommunIT.Shared.Portable.Extensions;
using Xunit;

namespace CommunIT.Shared.Portable.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("hello", false)]
        public void IsNullOrWhiteSpaceOrEmpty_returns_expected_from_theory(string input, bool expected)
        {
            var result = input.IsNullOrWhiteSpaceOrEmpty();

            Assert.Equal(expected, result);
        }
        
    }
}