using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace alexmore.Fx.Tests.Utils
{
    public class ReflectionUtilsTests
    {
        [Fact]
        public void ReadResourceFile_returns_resource_content()
        {
            Assert.Equal("This is Text Resource", this.GetType().GetTypeInfo().Assembly.ReadResourceFile("alexmore.Fx.Tests.Utils.TextResource.txt"));
        }
    }
}
