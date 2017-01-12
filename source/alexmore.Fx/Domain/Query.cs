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
using System.Collections.Generic;
using System.Threading.Tasks;
using alexmore.Fx.Collections;

namespace alexmore.Fx.Domain
{
    public class Query<T, TParameters> : QueryBase, IQuery<T, TParameters>, IQueryResult<T>
    {
        protected Query(IQueryDataSource dataSource) : base(dataSource) { }

        private TParameters _parameters;

        public IQueryResult<T> Execute(TParameters parameters)
        {
            _parameters = parameters;
            return this;
        }

        protected virtual Task<int> ProcessCountAsync(TParameters p) { throw new NotSupportedException($"Запрос {this.GetType().ToString()} не поддерживает метод {nameof(ProcessCountAsync)}."); }
        protected virtual Task<T> ProcessFirstOrDefaultAsync(TParameters p) { throw new NotSupportedException($"Запрос {this.GetType().ToString()} не поддерживает метод {nameof(ProcessFirstOrDefaultAsync)}."); }
        protected virtual Task<IReadOnlyList<T>> ProcessToListAsync(TParameters p) { throw new NotSupportedException($"Запрос {this.GetType().ToString()} не поддерживает метод {nameof(ProcessToListAsync)}."); }
        protected virtual Task<PagedList<T>> ProcessToPagedListAsync(TParameters p, int page, int pageSize) { throw new NotSupportedException($"Запрос {this.GetType().ToString()} не поддерживает метод {nameof(ProcessToPagedListAsync)}."); }

        public Task<int> CountAsync() => ProcessCountAsync(_parameters);
        public Task<T> FirstOrDefaultAsync() => ProcessFirstOrDefaultAsync(_parameters);
        public Task<IReadOnlyList<T>> ToListAsync() => ProcessToListAsync(_parameters);
        public Task<PagedList<T>> ToPagedListAsync(int page, int pageSize) => ProcessToPagedListAsync(_parameters, page, pageSize);

        public IQueryResult<TProjection> As<TProjection>()
        {
            throw new NotSupportedException($"Запрос {this.GetType().ToString()} не поддерживает проекцию данных.");
        }
    }
}
