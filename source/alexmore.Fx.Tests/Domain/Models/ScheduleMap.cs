using alexmore.Fx.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace alexmore.Fx.Tests.Domain.Models
{
    public class ScheduleMap : EntityMap<Schedule>
    {
        public override void Map(EntityTypeBuilder<Schedule> entity)
        {
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.Date).IsRequired().ValueGeneratedOnAdd();
            entity.Property(x => x.Title).IsRequired();
            entity.Property(x => x.Completed).IsRequired().HasDefaultValue(false);

            entity.HasOne(x => x.User).WithMany(x => x.Schedules).HasForeignKey(x => x.UserId).OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
        }
    }
}
