using System;
using alexmore.Fx.Domain;
using StructureMap;
using alexmore.Fx.Domain.Commands;
using alexmore.Fx.Data;

namespace alexmore.Fx.Tests.Domain.Infrastructure
{
    public class QueryResolver : IQueryResolver
    {
        public QueryResolver(IContainer container)
        {
            _container = container;
        }

        readonly IContainer _container;

        public IQuery<T, TParameters> Resolve<T, TParameters>(IQueryDataSource dataSource)
        {
            return _container.With(dataSource).GetInstance<IQuery<T, TParameters>>();
        }
    }

    public class CommandHandlerResolver : ICommandHandlerResolver
    {
        public CommandHandlerResolver(IContainer container)
        {
            _container = container;
        }

        readonly IContainer _container;

        public ICommandHandler<TCommand> Resolve<TCommand>(ICommandHandlerDataSource dataSource) where TCommand : ICommand
        {
            return _container.With(dataSource).GetInstance<ICommandHandler<TCommand>>();
        }

        public ICommandHandler<TCommand, TResult> Resolve<TCommand, TResult>(ICommandHandlerDataSource dataSource) where TCommand : ICommand
        {
            return _container.With(dataSource).GetInstance<ICommandHandler<TCommand, TResult>>();
        }
    }

    public class ValidatorResolver : IValidatorResolver
    {
        public ValidatorResolver(IContainer container)
        {
            _container = container;
        }

        readonly IContainer _container;


        public IValidator<T> Resolve<T>(ICommandHandlerDataSource dataSource)
        {
            var i = _container.With(dataSource).TryGetInstance<IValidator<T>>();
            return i;
        }
    }
}
