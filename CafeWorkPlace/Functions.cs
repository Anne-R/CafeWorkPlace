using CafeWorkPlace.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeWorkPlace
{
    public class Functions
    {
        CafeContext db = new CafeContext();
        public bool IsDigit(string str)
        {
            try
            {
                double d = Convert.ToDouble(str);
                return true;
            }
            catch (Exception exs)
            {
                return false;
            }

        }
        public bool AddingPosition(string title, string salary, string bonus)
        {
            if (!string.IsNullOrWhiteSpace(salary) && !string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(bonus)
                && IsDigit(salary) && IsDigit(bonus))
            {
                Position p = new Position
                {
                    Title = title,
                    Salary = Convert.ToDouble(salary),
                    SalesBonus = Convert.ToDouble(bonus)
                };
                db.Positions.Add(p);
                db.SaveChanges();

                var maxValue = db.Positions.Max(x => x.Id);
                var result = db.Positions.First(x => x.Id == maxValue);

                string w1 = result.Title;
                double w2 = result.Salary;
                double w4 = result.SalesBonus;
                
                return 0 == String.Compare(title, w1) && Convert.ToDouble(salary)==w2 && Convert.ToDouble(bonus) == w4;

            }
            else return false;
        }
        public bool AddingEmployee(string name, string pass, int pId)
        {
            if (!string.IsNullOrWhiteSpace(name) && pId>0 && IsDigit(pass))
            {
                Position pos = db.Positions.Find(pId);
                Employee emp = new Employee
                {
                    FullName = name,
                    Password = pass,
                    PositionId = pId,
                    Positions = pos
                };
                db.Employees.Add(emp);
                db.SaveChanges();

                var maxValue = db.Employees.Max(x => x.Id);
                var result = db.Employees.First(x => x.Id == maxValue);

                string w1 = result.FullName;
                string w2 = result.Password;
                int w3 = result.PositionId;

                return 0 == String.Compare(name, w1) && 0 == String.Compare(pass, w2) && Convert.ToInt32(pId) == w3;

            }
            else return false;
        }
        public bool AddingProdType(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                ProductType pt = new ProductType
                {
                    Title = title
                };
                db.ProductTypes.Add(pt);
                db.SaveChanges();

                var maxValue = db.ProductTypes.Max(x => x.Id);
                var result = db.ProductTypes.First(x => x.Id == maxValue);

                string w1 = result.Title;

                return 0 == String.Compare(title, w1);
            }
            else return false;
        }
        public bool AddingProduct(string title, string unit, int idType, double size, double price)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(unit) && idType>0)
            {
                ProductType pt = db.ProductTypes.Find(idType);
                Product prod = new Product
                {
                    Title = title,
                    Unit = unit,
                    TypeId = idType,
                    ProductTypes = pt,
                    PortionPrice = price,
                    PortionSize = size
                };
                db.Products.Add(prod);
                db.SaveChanges();

                var maxValue = db.Products.Max(x => x.Id);
                var result = db.Products.First(x => x.Id == maxValue);

                string w1 = result.Title;
                string w2 = result.Unit;
                int w3 = result.TypeId;
                double w4 = result.PortionPrice;
                double w5 = result.PortionSize;

                return 0 == String.Compare(title, w1) && 0 == String.Compare(unit, w2) && w3 == idType && w4==price && w5==size;
            }
            else return false;
        }
        public bool AddingStorageType(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                StorageType pt = new StorageType
                {
                    Title = title
                };
                db.StorageTypes.Add(pt);
                db.SaveChanges();

                var maxValue = db.StorageTypes.Max(x => x.Id);
                var result = db.StorageTypes.First(x => x.Id == maxValue);

                string w1 = result.Title;

                return 0 == String.Compare(title, w1);
            }
            else return false;
        }
        public bool AddingMenuType(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                MenuType pt = new MenuType
                {
                    Title = title
                };
                db.MenuTypes.Add(pt);
                db.SaveChanges();
                
                var maxValue = db.MenuTypes.Max(x => x.Id);
                var result = db.MenuTypes.First(x => x.Id == maxValue);

                string w1 = result.Title;

                return 0 == String.Compare(title, w1);
            }
            else return false;
        }
    }
}
