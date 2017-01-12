using alexmore.Fx.Domain;
using alexmore.Fx.Tests.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Queries
{
    public enum GetUserSchedulesRange
    {
        All = 0,
        Day = 1,
        Week = 2,
        Month = 3
    }

    /// <summary>
    /// Параметры запроса
    /// </summary>
    public class GetUserShedulesParameters
    {
        public int UserId { get; set; }
        public GetUserSchedulesRange Range { get; set; } = GetUserSchedulesRange.All;
    }

    /// <summary>    
    /// LinqQuery - класс, который прячет рутину по работе с БД.
    /// Вместо него может быть отдельная реализация IQuery<>, которая сможет брать данные где угодно. Хоть у черта на рогах ;-)
    /// Снаружи ничего не изменится.
    /// </summary>
    public class GetUserShedules : LinqQuery<Schedule, GetUserShedulesParameters>
    {
        // Через DI передеаем все с чем нужно взаимодействовать. 
        // Можно передать и DbConnection, правда для такого сценария есть ISqlDataSource
        public GetUserShedules(IQueryDataSource dataSource) : base(dataSource)
        {
        }

        // Process делает сложную работу и возвращает IQueryable
        // А есть еще ProcessAsync - если нужно материализовывать какие-либо данные.
        protected override IQueryable<Schedule> Process(GetUserShedulesParameters p)
        {
            var schedules = DataSource.Entities.Get<Schedule>(x => x.UserId == p.UserId);

            switch (p.Range)
            {
                case GetUserSchedulesRange.Day:
                    schedules = schedules.Where(x => x.Date.Date == DateTime.Now.Date);
                    break;
                case GetUserSchedulesRange.Week:
                    schedules = schedules.Where(x => x.Date.Date >= DateTime.Now.Date && x.Date <= DateTime.Now.Date.AddDays(7));
                    break;
                case GetUserSchedulesRange.Month:
                    schedules = schedules.Where(x => x.Date.Date >= DateTime.Now.Date && x.Date <= DateTime.Now.Date.AddMonths(1));
                    break;
            }

            return schedules;
        }
    }
}
