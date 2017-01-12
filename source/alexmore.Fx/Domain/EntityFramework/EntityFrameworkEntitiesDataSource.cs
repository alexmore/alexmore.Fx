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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace alexmore.Fx.Domain.EntityFramework
{
    public class EntityFrameworkReadOnlyEntitiesDataSource : IReadOnlyEntitiesDataSource
    {
        private readonly DbContext context;

        public EntityFrameworkReadOnlyEntitiesDataSource(DbContext context) { this.context = context; }

        public IQueryable<T> Get<T>() where T : class => context.Set<T>().AsNoTracking();
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class => context.Set<T>().AsNoTracking().Where(predicate);
    }

    public class EntityFrameworkEntitiesDataSource : IEntitiesDataSource
    {
        private readonly DbContext context;

        public EntityFrameworkEntitiesDataSource(DbContext context) { this.context = context; }

        public IQueryable<T> Get<T>() where T : class => context.Set<T>();
        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class => context.Set<T>().Where(predicate);
        public T Add<T>(T entity) where T : class => context.Set<T>().Add(entity).Entity;
        public T Attach<T>(T entity) where T : class => context.Set<T>().Attach(entity).Entity;
        public T Remove<T>(T entity) where T : class => context.Set<T>().Remove(entity).Entity;
        public void RemoveRange<T>(IEnumerable<T> entities) where T : class => context.Set<T>().RemoveRange(entities);
        public EntityEntry<T> Entry<T>(T entity) where T : class => context.Entry<T>(entity);
    }
}
