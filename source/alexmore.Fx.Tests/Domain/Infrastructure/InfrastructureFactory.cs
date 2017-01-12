using alexmore.Fx.Domain;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Infrastructure
{
    public class InfrastructureFactory
    {
        public InfrastructureFactory()
        {
            container = new Container(new DefaultRegistry());
        }

        private readonly IContainer container;

        public IDataSource CreateDataSource()
        {
            return container.GetInstance<IDataSource>();
        }
    }
}
