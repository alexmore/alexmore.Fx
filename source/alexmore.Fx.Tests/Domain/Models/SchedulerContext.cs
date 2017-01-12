using alexmore.Fx.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Models
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext(DbContextOptions options) : base(options) { }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурируем модель из маппингов *Map : EntityMap
            modelBuilder.AddEntityMapsFromAssembly(typeof(SchedulerContext).GetTypeInfo().Assembly);

            // А можно и задать правила именования элементов БД
            modelBuilder.ConfigureModelNames(typeName => StringUtils.ToSnakeCase(typeName));
        }

        public static SchedulerContext PopulateData(SchedulerContext ctx)
        {
            if (ctx.Users.Any()) return ctx;

            ctx.Users.Add(new User
            {
                Name = "Spider man",
                Schedules = {
                    new Schedule
                    {
                        Title = "Go to bank pay off student loans",
                        Date = DateTime.Now.AddHours(5)
                    },
                    new Schedule
                    {
                        Title = "Fight bad guys",
                        Date = DateTime.Now.AddHours(8)
                    },
                    new Schedule
                    {
                        Title = "Repair costume",
                        Date = DateTime.Now.AddDays(2)
                    },
                    new Schedule
                    {
                        Title = "Read scince book",
                        Date = DateTime.Now.AddDays(14)
                    }
                }

            });

            ctx.Users.Add(new User
            {
                Name = "Hulk",
                Schedules = {
                    new Schedule
                    {
                        Title = "Smash big rock into smaller rock",
                        Date = DateTime.Now.AddHours(3)
                    },
                    new Schedule
                    {
                        Title = "Take nap in cave",
                        Date = DateTime.Now.AddHours(5)
                    },
                    new Schedule
                    {
                        Title = "By more pants off amazon purple",
                        Date = DateTime.Now.AddDays(4)
                    },
                    new Schedule
                    {
                        Title = "Complete therapy journal",
                        Date = DateTime.Now.AddDays(10)
                    }
                }

            });

            ctx.Users.Add(new User
            {
                Name = "Superman",
                Schedules = {
                    new Schedule
                    {
                        Title = "Recycle",
                        Date = DateTime.Now.AddHours(1)
                    },
                    new Schedule
                    {
                        Title = "Find new job",
                        Date = DateTime.Now.AddHours(10)
                    },
                    new Schedule
                    {
                        Title = "Buy toilet paper",
                        Date = DateTime.Now.AddDays(1)
                    },
                    new Schedule
                    {
                        Title = "Fly to the sun",
                        Date = DateTime.Now.AddDays(21)
                    }
                }

            });

            ctx.SaveChanges();
            return ctx;
        }
    }
}
