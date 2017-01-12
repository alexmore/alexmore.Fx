using alexmore.Fx.Domain;
using alexmore.Fx.Tests.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Queries
{
    /// <summary>
    /// Параметры запроса
    /// </summary>
    public class FindUsersParameters
    {
        public string Name { get; set; }
        public bool? HasShedules { get; set; }
        public bool IncludeSchedules { get; set; } = false;
    }

    /// <summary>    
    /// LinqQuery - класс, который прячет рутину по работе с БД.
    /// Вместо него может быть отдельная реализация IQuery<>, которая сможет брать данные где угодно. Хоть у черта на рогах ;-)
    /// Снаружи ничего не изменится.
    /// </summary>
    public class FindUsers : LinqQuery<User, FindUsersParameters>
    {
        // Через DI передеаем все с чем нужно взаимодействовать. 
        // Можно передать и DbConnection, правда для такого сценария есть ISqlDataSource
        public FindUsers(IQueryDataSource dataSource) : base(dataSource)
        {
        }

        // Process делает сложную работу и возвращает IQueryable
        // А есть еще ProcessAsync - если нужно материализовывать какие-либо данные.
        protected override IQueryable<User> Process(FindUsersParameters p)
        {
            var users = DataSource.Entities.Get<User>();

            if (p.IncludeSchedules) users = users.Include(x => x.Schedules);

            if (p.Name.IsNotEmpty())
            {
                p.Name = p.Name.ToLower().Trim();
                users = users.Where(x => x.Name.ToLower().StartsWith(p.Name));
            }

            if (p.HasShedules.HasValue)
                users = users.Where(x => x.Schedules.Count > 0);

            return users;
        }
    }
}
