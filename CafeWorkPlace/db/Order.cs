using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employees { get; set; }

        public ICollection<Cheque> Cheques { get; set; }
    }
}
