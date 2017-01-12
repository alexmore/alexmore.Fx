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
using System.Threading.Tasks;

namespace alexmore.Fx.Domain
{
    public class LinqToEntitiesPredicateParameters
    {
        public LinqToEntitiesPredicateParameters(object predicateObject)
        {
            PredicateObject = predicateObject;
        }

        public object PredicateObject { get; set; }
    }

    public class LinqToEntitiesPredicateQuery<T, TParameters> : LinqQuery<T, LinqToEntitiesPredicateParameters> where T : class where TParameters : LinqToEntitiesPredicateParameters
    {
        public LinqToEntitiesPredicateQuery(IQueryDataSource dataSource) : base(dataSource) { }

        protected override Task<IQueryable<T>> ProcessAsync(LinqToEntitiesPredicateParameters parameters)
        {
            var entities = DataSource.Entities.Get<T>();

            var predicate = parameters.PredicateObject as Expression<Func<T, bool>>;
            if (predicate != null)
                entities = entities.Where(predicate);

            return Task.FromResult(entities);
        }
    }
}
