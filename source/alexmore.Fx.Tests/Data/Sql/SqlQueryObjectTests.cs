using alexmore.Fx.Collections;
using alexmore.Fx.Data;
using alexmore.Fx.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace alexmore.Fx.Tests.Data.Sql
{
    public class SqlQueryObjectTests
    {
        [Fact]
        public void Constructor_throws_ArgumentNullException_on_empty_sql_parameter()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlQueryObject(null));
        }
    }
}
