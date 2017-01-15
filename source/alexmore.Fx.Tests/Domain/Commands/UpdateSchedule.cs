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
    public class UpdateSchedule : Schedule, ICommand
    {
    }

    public class UpdateScheduleHandler : CommandHandler<UpdateSchedule>, IValidator<UpdateSchedule>
    {
        public UpdateScheduleHandler(ICommandHandlerDataSource dataSource) : base(dataSource)
        {
        }

        public Task<IEnumerable<ValidationMessage>> ValidateAsync(UpdateSchedule data)
        {
            var r = new List<ValidationMessage>();
            if (data.Title.IsEmpty()) r.Add(new ValidationMessage(nameof(data.Title), "Required"));
            return Task.FromResult(r.AsEnumerable());
        }

        protected override async Task HandleAsync(UpdateSchedule cmd)
        {
            var s = await DataSource.Entities.Get<Schedule>(x => x.Id == cmd.Id).SingleAsync();

            // TODO replace with automapper
            s.Title = cmd.Title;
        }
    }
}
