using CafeWorkPlace.db;
using CafeWorkPlace.KassWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CafeWorkPlace
{
    /// <summary>
    /// Логика взаимодействия для QuantityWin.xaml
    /// </summary>
    public partial class QuantityWin : Window
    {
        Functions f = new Functions();
        CafeContext db = KassWin.db;
        Cheque c = new Cheque();
        public QuantityWin(Cheque ch)
        {
            InitializeComponent();
            c = ch;
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbQ.Text))
            {
                if (int.TryParse(tbQ.Text, out var num)==true)
                {
                    List<Product> tempProd = db.Products.ToList();

                    var hdj = db.Products.Join(db.ComposCheques, i => i.Id, mi => mi.ProductId, (i, mi) => new { i.Id, mi.Quantity, mi.MenuId, i.LeftInStorage, mi.ProductId })
                      .Select(g => new { Id = g.Id, Quantity = g.Quantity, QiS = g.LeftInStorage, prod = g.ProductId }).ToList();

                    foreach (var product in tempProd)
                    {
                        product.LeftInStorage -= hdj.Where(x => x.prod == product.Id).Sum(s => s.Quantity*s.prod);
                    }

                    var stop = (from cc in db.ComposCheques
                                join m in db.Menu on cc.MenuId equals m.Id
                                join p in tempProd on cc.ProductId equals p.Id
                                join ch in db.Cheques on cc.ChequeId equals ch.Id
                                where cc.Quantity / ch.Quantity > p.LeftInStorage
                                select new
                                {
                                    IdCheque = ch.Id,
                                });

                    if (c != null)
                    {
                        if (stop.Any(o => o.IdCheque == c.Id) == false)
                        {
                            this.DialogResult = true;
                            
                        }
                        else MessageBox.Show("Недостаточно продуктов");
                    }
                }
                else MessageBox.Show("Введите значение цифрами");
            }
            else MessageBox.Show("Заполните поля");
        }
        public int Quantity
        {
            get { return Convert.ToInt32(tbQ.Text); }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
