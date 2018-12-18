using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommunIT.Shared.Portable.Extensions;
using Xunit;

namespace CommunIT.Shared.Portable.Tests
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void IsNullOrEmpty_on_IEnumerable_given_non_empty_list_returns_false()
        {
            var inputList = new List<int>{ 2,3,4 };

            var output = inputList.IsNullOrEmpty();
            
            Assert.False(output);
        }

        [Fact]
        public void IsNullOrEmpty_on_IEnumerable_given_null_list_returns_true()
        {
            IEnumerable<int> list = null;

            var output = list.IsNullOrEmpty();
            
            Assert.True(output);
        }

        [Fact]
        public void IsNullOrEmpty_on_IEnumerable_given_empty_list_returns_true()
        {
            var inputList = new List<int>();

            var output = inputList.IsNullOrEmpty();
            
            Assert.True(output);
        }
        
        [Fact]
        public void IsNullOrEmpty_on_IQueryable_given_non_empty_list_returns_false()
        {
            var inputList = new List<int>{ 2,3,4 }.AsQueryable();

            var output = inputList.IsNullOrEmpty();
            
            Assert.False(output);
        }

        [Fact]
        public void IsNullOrEmpty_on_IQueryable_given_null_list_returns_true()
        {
            IQueryable<int> list = null;

            var output = list.IsNullOrEmpty();
            
            Assert.True(output);
        }

        [Fact]
        public void IsNullOrEmpty_on_IQueryable_given_empty_list_returns_true()
        {
            var inputList = new List<int>().AsQueryable();

            var output = inputList.IsNullOrEmpty();
            
            Assert.True(output);
        }
        
        
    }
}