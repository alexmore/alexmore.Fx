using alexmore.Fx.Domain.Commands;
using alexmore.Fx.Tests.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Commands
{
    public class ExtendScheduleDate : Schedule, ICommand
    {
        public DateTime NewDate { get; set; }
    }

    public class ExtendScheduleDateHandle : CommandHandler<ExtendScheduleDate>
    {
        public ExtendScheduleDateHandle(ICommandHandlerDataSource dataSource) : base(dataSource)
        {
        }

        protected override async Task HandleAsync(ExtendScheduleDate cmd)
        {
            var s = await DataSource.Entities.Get<Schedule>(x => x.Id == cmd.Id).SingleAsync();
            s.Date = cmd.NewDate;
        }
    }
}
