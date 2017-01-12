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

using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using alexmore.Fx.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Domain
{
    public class LinqQueryProjectionResult<T, TProjection> : IQueryResult<TProjection>
    {
        public LinqQueryProjectionResult(Task<IQueryable<T>> data) { Data = data; }

        protected internal Task<IQueryable<T>> Data;

        public async Task<int> CountAsync() => await (await Data).ProjectTo<TProjection>().CountAsync().ConfigureAwait(false);
        public async Task<TProjection> FirstOrDefaultAsync() => await (await Data).ProjectTo<TProjection>().FirstOrDefaultAsync().ConfigureAwait(false);
        public async Task<IReadOnlyList<TProjection>> ToListAsync() => await (await Data).ProjectTo<TProjection>().ToListAsync().ConfigureAwait(false);
        public async Task<PagedList<TProjection>> ToPagedListAsync(int page, int pageSize) => await (await Data).ProjectTo<TProjection>().ToPagedListAsync(page, pageSize).ConfigureAwait(false);
        public IQueryResult<TNextProjection> As<TNextProjection>()
        {
            throw new NotSupportedException("Последовательные проекции данных не поддерживаются.");
        }
    }
}
