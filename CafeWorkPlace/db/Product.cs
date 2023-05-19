using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CafeWorkPlace.db
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double LeftInStorage { get; set; }
        public string Unit { get; set; }
        public double PortionSize { get; set; }
        public double PortionPrice { get; set; }
        
        public int TypeId { get; set; }
        public virtual ProductType ProductTypes { get; set; }

        public ICollection<Storage> Storage { get; set; }
        public ICollection<Composition> Compositions { get; set; }
        public ICollection<ComposCheque> ComposCheques { get; set; }

    }
}
