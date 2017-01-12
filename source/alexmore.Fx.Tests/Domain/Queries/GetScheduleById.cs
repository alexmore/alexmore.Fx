using alexmore.Fx.Domain;
using alexmore.Fx.Tests.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Queries
{
    /// <summary>
    /// Параметры запроса, по Id
    /// IdParameter<> позволяет получать данные не создавая IdParameter явно.
    /// Например, DataSource.Select<MyData>().WithId(id)
    /// LinqQuery - класс, который прячет рутину по работе с БД.
    /// Вместо него может быть отдельная реализация IQuery<>, которая сможет брать данные где угодно. Хоть у черта на рогах ;-)
    /// Снаружи ничего не изменится.
    /// </summary>
    public class GetScheduleById : LinqQuery<Schedule, IdParameter<int>>
    {
        // Через DI передеаем все с чем нужно взаимодействовать. 
        // Можно передать и DbConnection, правда для такого сценария есть ISqlDataSource
        public GetScheduleById(IQueryDataSource dataSource) : base(dataSource)
        {
        }

        // Process делает сложную работу и возвращает IQueryable
        // А есть еще ProcessAsync - если нужно материализовывать какие-либо данные.
        protected override IQueryable<Schedule> Process(IdParameter<int> p)
        {
            return DataSource.Entities.Get<Schedule>(x => x.Id == p.Id);
        }
    }
}
