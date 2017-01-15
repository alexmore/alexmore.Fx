using alexmore.Fx.Data;
using alexmore.Fx.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Domain
{
    public interface ICommandQueryFactory
    {
        QueryProcessor<T> Select<T>();
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> ExecuteAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand;
        Task<IEnumerable<ValidationMessage>> ValidateAsync<T>(T data);
    }
}
