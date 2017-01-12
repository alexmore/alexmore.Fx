#region License
/******************************************************************************
Copyright (c) 2016 Alexandr Mordvinov, alexandr.a.mordvinov@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
******************************************************************************/
#endregion

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace alexmore.Fx.Data.Entities
{
    public static class EntityMapExtensions
    {
        private static IEnumerable<Type> GetEntityMapTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes()
                .Where(x => !x.GetTypeInfo().IsAbstract && x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));
        }

        public static void AddEntityMapsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, bool skipIgnored = false)
        {
            var allMappingTypes = assembly.GetEntityMapTypes(typeof(IEntityMap<>));
            var mappingTypes = allMappingTypes;

            if (skipIgnored)
                mappingTypes = mappingTypes.Where(x => x.GetTypeInfo().GetCustomAttribute<MigrationIgnoreAttribute>().IsNull());

            foreach (var config in mappingTypes.Select(Activator.CreateInstance).Cast<IEntityMap>())
                config.Map(modelBuilder);

            if (skipIgnored)
            {
                foreach (var i in allMappingTypes.Where(x => x.GetTypeInfo().GetCustomAttribute<MigrationIgnoreAttribute>().IsNotNull()))
                    modelBuilder.Model.RemoveEntityType(i);
            }
        }

    }
}
