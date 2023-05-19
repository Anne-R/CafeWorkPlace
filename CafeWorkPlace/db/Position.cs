using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Salary { get; set; }
        public double SalesBonus { get; set; }


        public virtual ICollection<Employee> Employees { get; set; }
    }
}
