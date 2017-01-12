using Microsoft.EntityFrameworkCore;
using StructureMap;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using alexmore.Fx.Tests.Domain.Models;
using alexmore.Fx.Domain;
using alexmore.Fx.Domain.Commands;
using alexmore.Fx.Domain.EntityFramework;
using alexmore.Fx.Data;
using Moq;
using alexmore.Fx.Data.Sql;

namespace alexmore.Fx.Tests.Domain.Infrastructure
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<DbContextOptions>().Use(new DbContextOptionsBuilder().UseInMemoryDatabase().Options).Singleton();
            For<DbContext>().Use<SchedulerContext>(x => SchedulerContext.PopulateData(x.GetInstance<SchedulerContext>()));
            For<IConnectionFactory>().Use(x => new Mock<IConnectionFactory>().Object);
            For<IQueryResolver>().Use<QueryResolver>();
            For<ICommandHandlerResolver>().Use<CommandHandlerResolver>();
            For<IValidatorResolver>().Use<ValidatorResolver>();
            For<IDataSource>().Use<EntityFrameworkDataSource>();

            Scan(x =>
            {
                x.Assembly(typeof(EntityFrameworkDataSource).GetTypeInfo().Assembly);
                x.Assembly(typeof(DefaultRegistry).GetTypeInfo().Assembly);
                x.WithDefaultConventions();

                x.AddAllTypesOf(typeof(IQuery<,>));
                x.AddAllTypesOf(typeof(ICommandHandler<>));
                x.AddAllTypesOf(typeof(ICommandHandler<,>));
                x.AddAllTypesOf(typeof(IValidator<>));
            });
        }
    }
}
