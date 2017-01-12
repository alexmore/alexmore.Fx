using alexmore.Fx.Domain.Commands;
using alexmore.Fx.Tests.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Commands
{
    public class CompleteSchedule : Schedule, ICommand
    {
    }

    public class CompleteScheduleHandler : CommandHandler<CompleteSchedule>
    {
        public CompleteScheduleHandler(ICommandHandlerDataSource dataSource) : base(dataSource)
        {
        }

        protected override async Task HandleAsync(CompleteSchedule cmd)
        {
            var s = await DataSource.Entities.Get<Schedule>(x => x.Id == cmd.Id).SingleAsync();
            s.Complete();
            await DataSource.SaveChangesAsync();
        }
    }
}
