using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int PositionId { get; set; }
        public virtual Position Positions { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Storage> Storage { get; set; }
    }
}
