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
    public class StringUtilsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsEmpty_true_when_string_is_null_or_empty_whitespace(string s)
        {
            Assert.True(s.IsEmpty());
        }

        [Fact]
        public void IsEmpty_false_on_not_null_string()
        {
            Assert.False("some string".IsEmpty());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsNotEmpty_false_when_string_is_null_or_empty_whitespace(string s)
        {
            Assert.False(s.IsNotEmpty());
        }

        [Fact]
        public void IsNotEmpty_true_on_not_null_string()
        {
            Assert.True("some string".IsNotEmpty());
        }

        [Fact]
        public void ToIntSafely_returns_number()
        {
            Assert.Equal(12, "12".ToIntSafely());
        }

        [Fact]
        public void ToIntSafely_returns_null_by_default_on_fail()
        {
            Assert.Null("12s".ToIntSafely());
        }

        [Fact]
        public void ToIntSafely_returns_custom_default_value_on_fail()
        {
            Assert.Equal(5, "12s".ToIntSafely(5));
        }

        [Fact]
        public void AsGuid_returns_null_on_fail()
        {
            Assert.Null("12s".AsGuid());
        }

        [Fact]
        public void AsGuid_returns_guid_instance()
        {
            var g = Guid.NewGuid();
            Assert.Equal(g, g.ToString().AsGuid());
        }


        #region License
        // https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json.Tests/Utilities/StringUtilsTests.cs
        // Copyright (c) 2007 James Newton-King
        //
        // Permission is hereby granted, free of charge, to any person
        // obtaining a copy of this software and associated documentation
        // files (the "Software"), to deal in the Software without
        // restriction, including without limitation the rights to use,
        // copy, modify, merge, publish, distribute, sublicense, and/or sell
        // copies of the Software, and to permit persons to whom the
        // Software is furnished to do so, subject to the following
        // conditions:
        //
        // The above copyright notice and this permission notice shall be
        // included in all copies or substantial portions of the Software.
        //
        // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
        // OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
        // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        // WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
        // FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
        // OTHER DEALINGS IN THE SOFTWARE.
        #endregion
        [Fact]
        public void ToCamelCaseTest()
        {
            Assert.Equal("urlValue", StringUtils.ToCamelCase("URLValue"));
            Assert.Equal("url", StringUtils.ToCamelCase("URL"));
            Assert.Equal("id", StringUtils.ToCamelCase("ID"));
            Assert.Equal("i", StringUtils.ToCamelCase("I"));
            Assert.Equal("", StringUtils.ToCamelCase(""));
            Assert.Equal(null, StringUtils.ToCamelCase(null));
            Assert.Equal("person", StringUtils.ToCamelCase("Person"));
            Assert.Equal("iPhone", StringUtils.ToCamelCase("iPhone"));
            Assert.Equal("iPhone", StringUtils.ToCamelCase("IPhone"));
            Assert.Equal("i Phone", StringUtils.ToCamelCase("I Phone"));
            Assert.Equal("i  Phone", StringUtils.ToCamelCase("I  Phone"));
            Assert.Equal(" IPhone", StringUtils.ToCamelCase(" IPhone"));
            Assert.Equal(" IPhone ", StringUtils.ToCamelCase(" IPhone "));
            Assert.Equal("isCIA", StringUtils.ToCamelCase("IsCIA"));
            Assert.Equal("vmQ", StringUtils.ToCamelCase("VmQ"));
            Assert.Equal("xml2Json", StringUtils.ToCamelCase("Xml2Json"));
            Assert.Equal("snAkEcAsE", StringUtils.ToCamelCase("SnAkEcAsE"));
            Assert.Equal("snA__kEcAsE", StringUtils.ToCamelCase("SnA__kEcAsE"));
            Assert.Equal("snA__ kEcAsE", StringUtils.ToCamelCase("SnA__ kEcAsE"));
            Assert.Equal("already_snake_case_ ", StringUtils.ToCamelCase("already_snake_case_ "));
            Assert.Equal("isJSONProperty", StringUtils.ToCamelCase("IsJSONProperty"));
            Assert.Equal("shoutinG_CASE", StringUtils.ToCamelCase("SHOUTING_CASE"));
            Assert.Equal("9999-12-31T23:59:59.9999999Z", StringUtils.ToCamelCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("hi!! This is text. Time to test.", StringUtils.ToCamelCase("Hi!! This is text. Time to test."));
        }

        #region License
        // https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json.Tests/Utilities/StringUtilsTests.cs
        // Copyright (c) 2007 James Newton-King
        //
        // Permission is hereby granted, free of charge, to any person
        // obtaining a copy of this software and associated documentation
        // files (the "Software"), to deal in the Software without
        // restriction, including without limitation the rights to use,
        // copy, modify, merge, publish, distribute, sublicense, and/or sell
        // copies of the Software, and to permit persons to whom the
        // Software is furnished to do so, subject to the following
        // conditions:
        //
        // The above copyright notice and this permission notice shall be
        // included in all copies or substantial portions of the Software.
        //
        // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
        // OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
        // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        // WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
        // FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
        // OTHER DEALINGS IN THE SOFTWARE.
        #endregion
        [Fact]
        public void ToSnakeCaseTest()
        {
            Assert.Equal("url_value", StringUtils.ToSnakeCase("URLValue"));
            Assert.Equal("url", StringUtils.ToSnakeCase("URL"));
            Assert.Equal("id", StringUtils.ToSnakeCase("ID"));
            Assert.Equal("i", StringUtils.ToSnakeCase("I"));
            Assert.Equal("", StringUtils.ToSnakeCase(""));
            Assert.Equal(null, StringUtils.ToSnakeCase(null));
            Assert.Equal("person", StringUtils.ToSnakeCase("Person"));
            Assert.Equal("i_phone", StringUtils.ToSnakeCase("iPhone"));
            Assert.Equal("i_phone", StringUtils.ToSnakeCase("IPhone"));
            Assert.Equal("i_phone", StringUtils.ToSnakeCase("I Phone"));
            Assert.Equal("i_phone", StringUtils.ToSnakeCase("I  Phone"));
            Assert.Equal("i_phone", StringUtils.ToSnakeCase(" IPhone"));
            Assert.Equal("i_phone", StringUtils.ToSnakeCase(" IPhone "));
            Assert.Equal("is_cia", StringUtils.ToSnakeCase("IsCIA"));
            Assert.Equal("vm_q", StringUtils.ToSnakeCase("VmQ"));
            Assert.Equal("xml2_json", StringUtils.ToSnakeCase("Xml2Json"));
            Assert.Equal("sn_ak_ec_as_e", StringUtils.ToSnakeCase("SnAkEcAsE"));
            Assert.Equal("sn_a__k_ec_as_e", StringUtils.ToSnakeCase("SnA__kEcAsE"));
            Assert.Equal("sn_a__k_ec_as_e", StringUtils.ToSnakeCase("SnA__ kEcAsE"));
            Assert.Equal("already_snake_case_", StringUtils.ToSnakeCase("already_snake_case_ "));
            Assert.Equal("is_json_property", StringUtils.ToSnakeCase("IsJSONProperty"));
            Assert.Equal("shouting_case", StringUtils.ToSnakeCase("SHOUTING_CASE"));
            Assert.Equal("9999-12-31_t23:59:59.9999999_z", StringUtils.ToSnakeCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("hi!!_this_is_text._time_to_test.", StringUtils.ToSnakeCase("Hi!! This is text. Time to test."));
        }
    }
}
