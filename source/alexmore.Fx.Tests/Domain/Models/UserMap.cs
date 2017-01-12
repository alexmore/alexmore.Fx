using alexmore.Fx.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace alexmore.Fx.Tests.Domain.Models
{
    public class UserMap : EntityMap<User>
    {
        public override void Map(EntityTypeBuilder<User> entity)
        {
            entity.Property(x => x.Name).IsRequired();
        }
    }
}
