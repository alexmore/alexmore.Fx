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
using System.Linq.Expressions;
using alexmore.Fx.Domain.Exceptions;

namespace alexmore.Fx.Domain
{
    public class QueryProcessor<T>
    {
        public QueryProcessor(IQueryResolver resolver, IQueryDataSource dataSource)
        {
            _resolver = resolver;
            _dataSource = dataSource;
        }

        readonly IQueryResolver _resolver;
        readonly IQueryDataSource _dataSource;

        private IQuery<T, TParameters> Resolve<TParameters>(IQueryDataSource dataSource)
        {
            try
            {
                var handler = _resolver.Resolve<T, TParameters>(dataSource);
                return handler;
            }
            catch (Exception e)
            {
                if (typeof(TParameters) == typeof(LinqToEntitiesPredicateParameters))
                    throw new QueryNotFoundException(@"Не удалось найти обработчки запросов Linq to Entities. Убедитесь, что msc.Fx включена в список сборок, доступных для IQueryResolver.", e);

                throw new QueryNotFoundException($"Не удалось найти реализацию IQuery<{typeof(T).ToString()}, {typeof(TParameters).ToString()}>.", e);
            }
        }

        public IQueryResult<T> With<TParameters>(TParameters parameters)
        {
            return Resolve<TParameters>(_dataSource).Execute(parameters);
        }

        public IQueryResult<T> Where(Expression<Func<T, bool>> predicate)
        {
            return Resolve<LinqToEntitiesPredicateParameters>(_dataSource).Execute(new LinqToEntitiesPredicateParameters(predicate));
        }

        public IQueryResult<T> All()
        {
            return Resolve<LinqToEntitiesPredicateParameters>(_dataSource).Execute(new LinqToEntitiesPredicateParameters(null));
        }
    }
}
