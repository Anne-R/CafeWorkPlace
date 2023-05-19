using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class Storage
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Quantity { get; set; }

        public int TypeId { get; set; }
        public virtual StorageType StorageTypes { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employees { get; set; }
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }
    }
}
