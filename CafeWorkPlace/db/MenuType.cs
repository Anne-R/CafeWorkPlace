using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class MenuType
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Menu> Menu { get; set; }
    }
}
