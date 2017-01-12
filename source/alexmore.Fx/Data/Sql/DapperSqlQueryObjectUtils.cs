#region License
/******************************************************************************
Copyright (c) 2016 Alexandr Mordvinov, alexandr.a.mordvinov@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
******************************************************************************/
#endregion

using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Threading.Tasks;

namespace alexmore.Fx.Data.Sql
{
    public static class DapperSqlQueryObjectUtils
    {
        public static IEnumerable<T> Query<T>(this IDbConnection c, SqlQueryObject queryObject, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return c.Query<T>(queryObject.Sql, queryObject.QueryParams, transaction, buffered, commandTimeout, commandType);
        }

        public static int Execute(this IDbConnection c, SqlQueryObject queryObject, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return c.Execute(queryObject.Sql, queryObject.QueryParams, transaction, commandTimeout, commandType);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection c, SqlQueryObject queryObject, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await c.QueryAsync<T>(queryObject.Sql, queryObject.QueryParams, transaction, commandTimeout, commandType).ConfigureAwait(false);
        }

        public static async Task<int> ExecuteAsync(this IDbConnection c, SqlQueryObject queryObject, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await c.ExecuteAsync(queryObject.Sql, queryObject.QueryParams, transaction, commandTimeout, commandType).ConfigureAwait(false);
        }
    }
}
