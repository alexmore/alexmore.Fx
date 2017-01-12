using System;
using Xunit;

namespace alexmore.Fx.Tests.Utils
{
    public class DateTimeUtilsTests
    {
        [Fact]
        public void ToUnixEpochDate_converts_correct()
        {
            Assert.Equal(1465519607, new DateTime(2016, 6, 10, 7, 46, 47).ToUnixEpochDate());
        }

        [Fact]
        public void ToShortDateString_correct()
        {
            var dateTime = DateTime.Now;
            Assert.Equal(dateTime.ToString("d"), dateTime.ToShortDateString());
        }

        [Fact]
        public void ToShortTimeString_correct()
        {
            var dateTime = DateTime.Now;
            Assert.Equal(dateTime.ToString("t"), dateTime.ToShortTimeString());
        }

        [Fact]
        public void ToLongDateString_correct()
        {
            var dateTime = DateTime.Now;
            Assert.Equal(dateTime.ToString("D"), dateTime.ToLongDateString());
        }

        [Fact]
        public void ToLongTimeString_correct()
        {
            var dateTime = DateTime.Now;
            Assert.Equal(dateTime.ToString("T"), dateTime.ToLongTimeString());
        }
    }
}
