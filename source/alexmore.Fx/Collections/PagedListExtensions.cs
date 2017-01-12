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

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Collections
{
    public static class PagedListExtensions
    {
        public static IQueryable<T> SelectPage<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize)
        {
            if (page <= 0) page = 1;

            var itemsCount = await source.CountAsync().ConfigureAwait(false);
            var pageItems = await source.SelectPage(page, pageSize).ToListAsync().ConfigureAwait(false);
            return new PagedList<T>(
                pageSize,
                page,
                itemsCount,
                pageItems);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize)
        {
            if (page <= 0) page = 1;

            var itemsCount = source.Count();
            var pageItems = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(
                pageSize,
                page,
                itemsCount,
                pageItems);
        }
    }
}
