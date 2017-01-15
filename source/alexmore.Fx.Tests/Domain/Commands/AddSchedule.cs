using alexmore.Fx.Data;
using alexmore.Fx.Domain.Commands;
using alexmore.Fx.Tests.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Commands
{
    public class AddSchedule : Schedule, ICommand { }

    public class AddScheduleHandler : CommandHandler<AddSchedule, Schedule>, IValidator<AddSchedule>
    {
        public AddScheduleHandler(ICommandHandlerDataSource dataSource) : base(dataSource)
        {
        }

        protected override Task<Schedule> HandleAsync(AddSchedule cmd)
        {
            var s = new Schedule()
            {
                Title = cmd.Title,
                Date = cmd.Date,
                UserId = cmd.UserId
            };

            var r = DataSource.Entities.Add(s);

            return Task.FromResult(r);
        }

        public async Task<IEnumerable<ValidationMessage>> ValidateAsync(AddSchedule data)
        {
            var r = new List<ValidationMessage>();
            if (!(await DataSource.Entities.Get<User>(x => x.Id == data.UserId).AnyAsync())) r.Add(new ValidationMessage(nameof(data.UserId), "Пользователь не найден"));
            if (data.Title.IsEmpty()) r.Add(new ValidationMessage(nameof(data.Title), "Required"));
            if (data.Date < new DateTime(1999, 1, 1)) r.Add(new ValidationMessage(nameof(data.Date), "Greater 01.01.1999"));
            return r;
        }
    }

}
