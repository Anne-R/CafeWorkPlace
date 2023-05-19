using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace.db
{
    public class CafeContext :DbContext
    {
        public CafeContext()
            :base("DBConnection")
        { }

        public DbSet<StorageType> StorageTypes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<MenuType> MenuTypes { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<ComposCheque> ComposCheques { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasRequired(c => c.MenuTypes)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ComposCheque>()
                .HasRequired(c => c.Menu)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
