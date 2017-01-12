using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alexmore.Fx.Tests.Domain.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; } = false;

        public virtual User User { get; set; }

        /// <summary>
        /// Крутые разработчики не должны использовать анемичную модель
        /// Все по DDD :-)
        /// </summary>
        public void Complete()
        {
            Completed = true;
        }
    }
}
