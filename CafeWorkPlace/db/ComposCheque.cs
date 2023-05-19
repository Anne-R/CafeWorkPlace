using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class ComposCheque
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public int Portions { get; set; }

        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        public int ProductId { get; set; }
        public virtual Product Products { get; set; }

        public int ChequeId { get; set; }
        public virtual Cheque Cheques { get; set; }
    }
}
