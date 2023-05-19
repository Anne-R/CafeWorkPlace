using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Size { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }

        public int TypeId { get; set; }
        public  MenuType MenuTypes { get; set; }


        public ICollection<Composition> Compositions { get; set; }
        public ICollection<Cheque> Cheques { get; set; }
        public ICollection<ComposCheque> ComposCheques { get; set; }
    }
}
