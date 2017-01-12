using System.Collections.Generic;

namespace alexmore.Fx.Tests.Domain.Models
{
    public class User
    {
        public User()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
