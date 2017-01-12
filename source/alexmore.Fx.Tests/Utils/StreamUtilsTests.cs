using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace alexmore.Fx.Tests.Utils
{
    public class StreamUtilsTests
    {
        private Stream StringToStream(params string[] s)
        {
            var sb = new StringBuilder();
            foreach (var i in s) sb.AppendLine(i);

            var textStream = new MemoryStream();
            var textStreamSW = new StreamWriter(textStream);
            textStreamSW.Write(sb.ToString().Trim());
            textStreamSW.Flush();
            textStream.Position = 0;
            return textStream;
        }

        [Fact]
        public void ReadString_returns_correct_string()
        {
            Assert.Equal("text", StringToStream("text").ReadString());
        }

        [Fact]
        public async Task ReadStringAsync_returns_correct_string()
        {
            Assert.Equal("text", await StringToStream("text").ReadStringAsync());
        }

        [Fact]
        public void ReadAllLines_returns_correct_array()
        {
            var lines = new[] { "1", "2", "3" };

            Assert.Equal(lines, StringToStream(lines).ReadAllLines());
        }

        [Fact]
        public async Task ReadAllLinesAsync_returns_correct_array()
        {
            var lines = new[] { "1", "2", "3" };

            Assert.Equal(lines, await StringToStream(lines).ReadAllLinesAsync());
        }
    }
}
