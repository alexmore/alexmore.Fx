using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace alexmore.Fx.Tests.Utils
{
    public class LinqUtilsTests
    {
        private int[] numbers = new[] { 1, 2, 3 };
        private KeyValuePair<int, int>[] objects = new[] { new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(3, 4), new KeyValuePair<int, int>(5, 6), };

        [Fact]
        public void SortBy_Ascending()
        {
            Assert.Equal(numbers.OrderBy(x => x), numbers.OrderByDescending(x => x).AsQueryable().SortBy(x => x, Fx.Data.SortOrder.Asc));
        }

        [Fact]
        public void SortBy_Descending()
        {
            Assert.Equal(numbers.OrderByDescending(x => x), numbers.OrderBy(x => x).AsQueryable().SortBy(x => x, Fx.Data.SortOrder.Desc));
        }

        [Fact]
        public void OrderBy_property_name_string()
        {
            Assert.Equal(objects.OrderBy(x => x.Key), objects.AsQueryable().OrderBy("Key"));
        }

        [Fact]
        public void OrderByDescending_property_name_string()
        {
            Assert.Equal(objects.OrderByDescending(x => x.Key), objects.AsQueryable().OrderByDescending("Key"));
        }
    }
}
