using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Data.Entities
{
    public static class ModelBuilderUtils
    {
        public static void ConfigureModelNames(this ModelBuilder b, Func<string, string> mapNameFunc)
        {
            foreach (var i in b.Model.GetEntityTypes())
            {
                i.Relational().TableName = mapNameFunc(i.Relational().TableName);
                foreach (var p in i.GetProperties())
                {
                    p.Relational().ColumnName = mapNameFunc(p.Relational().ColumnName);
                }
            }
        }
    }
}
