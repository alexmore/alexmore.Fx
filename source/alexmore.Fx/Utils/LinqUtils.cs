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

using System;
using System.Linq;
using System.Linq.Expressions;
using alexmore.Fx.Data;

namespace alexmore.Fx
{
    public static class LinqUtils
    {
        public static IOrderedQueryable<T> SortBy<T, TKey>(this IQueryable<T> items, Expression<Func<T, TKey>> keySelector, SortOrder order)
        {
            return order == SortOrder.Desc ? items.OrderByDescending(keySelector) : items.OrderBy(keySelector);
        }

        public static IOrderedQueryable<T> ThenSortBy<T, TKey>(this IOrderedQueryable<T> items, Expression<Func<T, TKey>> keySelector, SortOrder order)
        {
            return order == SortOrder.Desc ? items.ThenByDescending(keySelector) : items.ThenBy(keySelector);
        }

        public static IOrderedQueryable<T> SortByPropertyName<T>(this IQueryable<T> items, SortValue sort)
        {
            return (IOrderedQueryable<T>)(sort.Order == SortOrder.Desc ? items.OrderByDescending(sort.Column) : items.OrderBy(sort.Column));
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> q, string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, propertyName);
            var exp = Expression.Lambda(prop, param);
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), "OrderBy", types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> q, string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, propertyName);
            var exp = Expression.Lambda(prop, param);
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), "OrderByDescending", types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
