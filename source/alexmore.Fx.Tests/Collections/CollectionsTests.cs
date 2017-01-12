using alexmore.Fx.Collections;
using alexmore.Fx.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace alexmore.Fx.Tests.Collections
{
    public class CollectionsTests
    {
        [Fact]
        public void ToPagedList_sets_page_to_1_when_less_or_equls_0()
        {
            Assert.Equal(1, new List<int>().AsQueryable().ToPagedList(0, 2).Page);
        }

        [Fact]
        public void SelectPage_skips_0_for_first_page()
        {
            Assert.Equal(1, new List<int>() { 1, 2, 3 }.AsQueryable().ToPagedList(1, 1).Items.First());
        }

        [Fact]
        public void SelectPage_select_second_page()
        {
            Assert.Equal(new[] { 3, 4 }, new List<int>() { 1, 2, 3, 4 }.AsQueryable().ToPagedList(2, 2).Items);
        }
    }
}
