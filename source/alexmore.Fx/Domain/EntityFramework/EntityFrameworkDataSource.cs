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
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Threading;
using alexmore.Fx.Data;
using alexmore.Fx.Domain.Commands;
using alexmore.Fx.Data.Sql;

namespace alexmore.Fx.Domain.EntityFramework
{
    public class EntityFrameworkDataSource : IDataSource, IQueryDataSource, ICommandHandlerDataSource
    {
        protected IQueryResolver QueryResolver { get; set; }
        protected ICommandHandlerResolver CommandHandlerResolver { get; set; }
        protected IValidatorResolver ValidatorResolver { get; set; }
        protected DbContext Context { get; set; }
        protected IConnectionFactory ConnectionFactory { get; set; }

        private readonly EntityFrameworkEntitiesDataSource entitiesDataSource;
        private readonly EntityFrameworkReadOnlyEntitiesDataSource readonlyEntitiesDataSource;

        public EntityFrameworkDataSource(
            DbContext context,
            IConnectionFactory connectionFactory,
            IQueryResolver queryResolver, ICommandHandlerResolver commandHandlerResolver, IValidatorResolver valdiatorResolver
            )
        {
            Context = context;
            ConnectionFactory = connectionFactory;

            QueryResolver = queryResolver;
            CommandHandlerResolver = commandHandlerResolver;
            ValidatorResolver = valdiatorResolver;

            entitiesDataSource = new EntityFrameworkEntitiesDataSource(Context);
            readonlyEntitiesDataSource = new EntityFrameworkReadOnlyEntitiesDataSource(Context);
        }

        #region IDataSource
        private ICommandHandler<TCommand> GetComandHandler<TCommand>() where TCommand : ICommand
        {
            var handler = CommandHandlerResolver.Resolve<TCommand>(this);

            if (handler == null)
                throw new CommandHandlerNotFoundException($"Не наден обработчик комманд для {typeof(TCommand).ToString()}.");

            return handler;
        }

        private ICommandHandler<TCommand, TResult> GetComandHandler<TCommand, TResult>() where TCommand : ICommand
        {
            var handler = CommandHandlerResolver.Resolve<TCommand, TResult>(this);

            if (handler == null)
                throw new CommandHandlerNotFoundException($"Не наден обработчик комманд для {typeof(TCommand).ToString()}.");

            return handler;
        }

        private IValidator<T> GetValidator<T>()
        {
            return ValidatorResolver.Resolve<T>(this);
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = GetComandHandler<TCommand>();

            var validationResult = await ValidateAsync(command).ConfigureAwait(false);
            if (validationResult.IsNotNull() && validationResult.Any()) throw new ValidationException($"Ошибка валидации для {handler.GetType().ToString()}", validationResult);

            await handler.ExecuteAsync(command);
        }

        public async Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            var handler = GetComandHandler<TCommand, TResult>();

            var validationResult = await ValidateAsync(command).ConfigureAwait(false);
            if (validationResult.IsNotNull() && validationResult.Any()) throw new ValidationException($"Ошибка валидации для {handler.GetType().ToString()}", validationResult);

            return await handler.ExecuteAsync(command);
        }

        public QueryProcessor<T> Select<T>() => new QueryProcessor<T>(QueryResolver, this);

        public async Task<IEnumerable<ValidationMessage>> ValidateAsync<T>(T data)
        {
            var validator = GetValidator<T>();

            if (validator.IsNull()) return null;

            return await validator.ValidateAsync(data).ConfigureAwait(false);
        }
        #endregion

        #region IQueryDataSource
        IReadOnlyEntitiesDataSource IQueryDataSource.Entities => readonlyEntitiesDataSource;
        #endregion

        #region ICommandHandlerDataSource
        public IDbContextTransaction BeginTransaction() => Context.Database.BeginTransaction();

        public int SaveChanges() => Context.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => Context.SaveChangesAsync(cancellationToken);

        public IEntitiesDataSource Entities { get { return entitiesDataSource; } }
        #endregion

        #region ISqlDataSource
        DbConnection ISqlDataSource.Connection => Context.Database.GetDbConnection();

        T ISqlDataSource.UsingConnection<T>(Func<DbConnection, T> func)
        {
            using (var c = ConnectionFactory.Create())
            {
                c.Open();
                return func(c);
            }
        }

        async Task<T> ISqlDataSource.UsingConnectionAsync<T>(Func<DbConnection, Task<T>> func)
        {
            using (var c = ConnectionFactory.Create())
            {
                await c.OpenAsync();
                return await func(c);
            }
        }
        #endregion

        #region IDisposable pattern
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);


        #endregion
    }
}
