using alexmore.Fx.Data;
using alexmore.Fx.Domain;
using alexmore.Fx.Tests.Domain.Commands;
using alexmore.Fx.Tests.Domain.Models;
using alexmore.Fx.Tests.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace alexmore.Fx.Tests.Domain
{
    /// <summary>
    /// Пример того, как может выглядеть контроллер WebAPI при использовании DDD без Repository
    /// Предметная область: планировщик
    /// </summary>
    public class WebApiControllerSample
    {
        /// <summary>
        /// Источник данных/Шина/Другое крутое название 
        /// </summary>
        private IDataSource dataSource = new Infrastructure.InfrastructureFactory().CreateDataSource();

        /// <summary>
        /// Метод HttpGet
        /// Возвращает пользотваелей по критериям
        /// </summary>
        /// <returns></returns>
        [Theory, InlineData("s")]
        public async Task GetUsersAsync(string name)
        {
            // FindUsersParameters - критерий отбора пользователей
            // За каждмы Select<>().With<>() спрятана реализация IQuery<>, которая и возвращает данные.
            // Прелесть в том, что нет общего репозитория с кучей методов GetXXX, SetXXX, UpdateXXX
            // Все спрятано в отдельных классах. Прям сплошной SRP.
            // А еще прелесть в том, что в любой момент IQuery может быть заменен, и никто этого не заметит. 
            // А ведь так можно заменить SQL базу на Redis или вообще переехать на NoSQL. Хоть частично, хоть всем разом.
            // IoC разрулит все зависимости автоматом.
            var users = await dataSource.Select<User>().With(new FindUsersParameters { Name = name, IncludeSchedules = true }).ToListAsync();

            Assert.NotEmpty(users);
            Assert.NotEmpty(users.SelectMany(x => x.Schedules));
        }

        [Theory, InlineData(1)]
        public async Task GetScheduleById(int id)
        {
            // Аналогично GetUsersAsync, только выбирается одна запись
            var schedule = await dataSource.Select<Schedule>().WithId(id).FirstOrDefaultAsync();

            Assert.NotNull(schedule);
        }

        [Theory, InlineData(1)]
        public async Task GetUserSchedules(int userId)
        {
            var schedules = await dataSource.Select<Schedule>().With(new GetUserShedulesParameters { UserId = userId }).ToListAsync();

            Assert.NotEmpty(schedules);
        }

        /// <summary>
        /// Пометка задачи, как завершенной
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Theory, InlineData(1)]
        public async Task CompleteSchedule(int id)
        {
            // По приципу CQRS - запросы возвращают, команды - исзменяют.
            // CompleteSchedule - команда
            // CompleteScheduleHandler - обработчик команды.
            // ExecuteAsync находит обработчик и вызывает метод Execute
            // Но это еще не все! Если реализовать IValidator<TCommand>, 
            // то перед выполнением Execute будет выполнена валидация.
            // Команды - это не CRUD. Это изменения системы по бизнес-правилам. Зарегистрировать заказ, сопроводить и т.д.
            // Если используется анемичная модель - то все делается в командах, иначе команды просто обертки над методами модели.
            // А еще команды можно использовать как Unit of Work.
            await dataSource.ExecuteAsync(new CompleteSchedule() { Id = id });
            await dataSource.SaveChangesAsync();

            var completedS = await dataSource.Select<Schedule>().WithId(id).FirstOrDefaultAsync();

            Assert.NotNull(completedS);
            Assert.True(completedS.Completed);
        }

        [Theory, InlineData(1)]
        public async Task ExtendScheduleDate(int id)
        {
            var dt = new DateTime(1900, 1, 1);

            await dataSource.ExecuteAsync(new ExtendScheduleDate() { Id = id, NewDate = dt });
            await dataSource.SaveChangesAsync();

            var completedS = await dataSource.Select<Schedule>().WithId(id).FirstOrDefaultAsync();

            Assert.Equal(dt, completedS.Date);
        }

        [Theory, InlineData(1)]
        public async Task UpdateSchdule_validation_fail_on_empty_title(int id)
        {
            await Assert.ThrowsAsync<ValidationException>(() => dataSource.ExecuteAsync(new UpdateSchedule() { Id = id, Title = "" }));
        }

        [Theory, InlineData(1, "New Title")]
        public async Task UpdateSchdule(int id, string title)
        {
            await dataSource.ExecuteAsync(new UpdateSchedule() { Id = id, Title = title });
            await dataSource.SaveChangesAsync();

            var completedS = await dataSource.Select<Schedule>().WithId(id).FirstOrDefaultAsync();

            Assert.Equal(title, completedS.Title);
        }

        [Theory, InlineData(1)]
        public async Task AddSchedule(int userId)
        {
            // Также есть команды которые могут вернуть некоторое значение. Это нарушает принцип CQRS,
            // но для увеличения производительности в небольших системах очень удобно возвращать исправленные или
            // сгенерированные данные
            var added = await dataSource.ExecuteAsync<AddSchedule, Schedule>(new AddSchedule { Title = "New Schedule", Date = DateTime.Now, UserId = 1 });
            await dataSource.SaveChangesAsync();

            var s = await dataSource.Select<Schedule>().WithId(added.Id).FirstOrDefaultAsync();

            Assert.NotNull(s);
            Assert.Equal(userId, s.UserId);
        }


    }
}
