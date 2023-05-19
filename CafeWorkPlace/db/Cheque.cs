using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class Cheque
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Size { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }

        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        public int OrderId { get; set; }
        public virtual Order Orders { get; set; }

        public ICollection<ComposCheque> ComposCheques { get; set; }
    }
}
