using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace alexmore.Fx.Tests.Utils
{
    public class ObjectUtilsTests
    {
        [Fact]
        public void IsNull_IsNotNull_on_null_object()
        {
            object obj = null;
            Assert.True(obj.IsNull() && !obj.IsNotNull());
        }

        [Fact]
        public void IsNull_IsNotNull_false_on_not_null_object()
        {
            object obj = new object();
            Assert.True(!obj.IsNull() && obj.IsNotNull());
        }

        [Fact]
        public void IsNull_IsNotNull_true_on_null_nullable()
        {
            Nullable<int> obj = null;
            Assert.True(obj.IsNull() && !obj.IsNotNull());
        }

        [Fact]
        public void IsNull_IsNotNull_false_on_not_null_nullable()
        {
            Nullable<int> obj = 12;
            Assert.True(!obj.IsNull() && obj.IsNotNull());
        }
    }
}
