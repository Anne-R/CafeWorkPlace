using CafeWorkPlace.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using Xceed.Wpf.AvalonDock.Controls;
using Menu = CafeWorkPlace.db.Menu;

namespace CafeWorkPlace.KassWindows
{
    /// <summary>
    /// Логика взаимодействия для KassWin.xaml
    /// </summary>
    public partial class KassWin : Window
    {
        public static CafeContext db = new CafeContext();
        public static int IdCheque;
        public MenuType selectedType;
        public List<Cheque> cheques = new List<Cheque>();
        private List<Cheque> cheques_copy = new List<Cheque>();
        public List<ComposCheque> composCheques = new List<ComposCheque>();
        static Order order = new Order();
        //public int currentOrder = order.Id;
        
        public KassWin()
        {
            InitializeComponent();

            order.EmployeeId = AutorizWIn.idWorker;
            order.Total = 0;
            lbTypes.ItemsSource = db.MenuTypes.ToList();
            lbMenu.ItemsSource = db.Menu.Where(x=>x.IsActive).ToList();
            tbWorker.Text=(db.Employees.Find(AutorizWIn.idWorker)).FullName;
        }

        private void UpDate()
        {
            List<Menu> listMenu = db.Menu.Where(x => x.IsActive).ToList();
            string text = tbSearch.Text;

            if(selectedType!=null)
                listMenu = db.Menu.Where(x => x.IsActive && x.TypeId == selectedType.Id).ToList();
            else listMenu = db.Menu.Where(x => x.IsActive).ToList();

            if (!string.IsNullOrWhiteSpace(text))
                listMenu = listMenu.Where(x => x.Title.ToLower().Contains(text.ToLower())).ToList();

            lbMenu.ItemsSource = listMenu;
        }
        
        //private bool IsSame(List<ComposCheque> orig, List<ComposCheque> curr)
        //{
        //    bool flag = true;

        //    int l1 = orig.Count;
        //    int l2 = curr.Count;

        //    if(l1 != l2)
        //        flag = false;
        //    else
        //    {
        //        int l = Math.Max(l1, l2);
        //        for (int i = 0; i <= l; i++)
        //        {
        //            ComposCheque c1 = orig[i];
        //            c1.Portions = 0;
        //            ComposCheque c2 = curr[i];
        //            c2.Portions = 0;
        //            if (c1!=c2)
        //                flag = false;
        //        }
        //    }
        //    return flag;
        //}

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            selectedType = (((sender as Button).DataContext) as MenuType);
            UpDate();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lbMenu.ItemsSource = db.Menu.Where(x => x.IsActive).ToList();
        }

        private void btnToCheque_Click(object sender, RoutedEventArgs e)
        {
            Menu selectedMsenu = (((sender as Button).DataContext) as Menu);
            if (selectedMsenu != null)
            {
                var stop1 = (from mi in db.Compositions
                             join m in db.Menu on mi.MenuId equals m.Id
                             join i in db.Products on mi.ProductId equals i.Id
                             where mi.Quantity > i.LeftInStorage
                             select new
                             {
                                 idMenu = m.Id,
                                 idProd = i.Id,
                                 q = mi.Quantity
                             });

                if (stop1.Any(o => o.idMenu == selectedMsenu.Id) == false)
                {
                    //List<ComposCheque> curr = db.Compositions.Where(x=>x.MenuId == selectedMsenu.Id).ToList();
                    if (cheques.Any(u => u.MenuId == selectedMsenu.Id && u.Comment==null))
                    {
                        foreach (var item in cheques)
                        {
                            if (item.MenuId == selectedMsenu.Id)
                            {
                                item.Quantity++;
                                foreach(var sos in composCheques.Where(x => x.MenuId == selectedMsenu.Id))
                                {
                                    sos.Portions++;
                                }
                            }
                        }
                    }
                    else
                    {
                        Cheque ch = new Cheque
                        {
                            Menu = selectedMsenu,
                            MenuId = selectedMsenu.Id,
                            Size = 1,
                            OrderId = order.Id,
                            Orders = order,
                            Quantity = 1,
                            Price = selectedMsenu.Price
                        };

                        IdCheque = ch.Id;
                        cheques.Add(ch);
                                            


                        var t = db.Compositions.Where(x => x.MenuId == selectedMsenu.Id).ToList();

                        foreach (var com in t)
                        {
                            ComposCheque c = new ComposCheque
                            {
                                ChequeId = IdCheque,
                                Cheques = ch,
                                Menu = selectedMsenu,
                                MenuId = selectedMsenu.Id,
                                Portions = 1
                            };
                            c.ProductId = com.ProductId;
                            c.Quantity = com.Quantity;
                            composCheques.Add(c);
                        }
                    }
                    cheques_copy.Clear();
                    cheques_copy = cheques.ToList();
                    dtgOrder.ItemsSource = cheques_copy;
                    tbTotal.Text = cheques.Sum(x => x.Price * x.Quantity).ToString();
                }
                else MessageBox.Show("Не хватает ингридиентов, выберите другую позицию");
            }
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            AutorizWIn autorizWIn = new AutorizWIn();
            autorizWIn.Show();
            this.Close();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpDate();
        }

        private void btnComposition_Click(object sender, RoutedEventArgs e)
        {
            Cheque c = dtgOrder.SelectedItem as Cheque;
            if(c!=null)
            {
                IdCheque = c.Id;
                ChangeComposWin ec = new ChangeComposWin(c, composCheques);
                if (ec.ShowDialog() == true)
                {
                    var rem = composCheques.Where(x => x.ChequeId == c.Id).ToList();
                    foreach (var rem2 in rem)
                         composCheques.Remove(rem2);

                    foreach(var i in ec.lst)
                    {
                        composCheques.Add(i);
                    }

                    cheques_copy.Clear();
                    cheques_copy = cheques.ToList();
                    dtgOrder.ItemsSource = cheques_copy;
                    tbTotal.Text = c.Price.ToString();
                    MessageBox.Show("Изменения сохранены");
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Cheque perem = dtgOrder.SelectedItem as Cheque;
            if (perem != null)
            {
                var rem = composCheques.Where(x => x.ChequeId == perem.Id).ToList();
                foreach (var rem2 in rem)
                    composCheques.Remove(rem2);

                cheques.Remove(perem);

                cheques_copy.Clear();
                cheques_copy = cheques.ToList();
                dtgOrder.ItemsSource = cheques_copy;
                tbTotal.Text = cheques.Sum(x => x.Price * x.Quantity).ToString();
            }
            else MessageBox.Show("Выберите позицию, которую хотите удалить");
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if (cheques.Any())
            {
                order.Total = Convert.ToDouble(tbTotal.Text);
                order.OrderDate = DateTime.Now;
                order.EmployeeId = AutorizWIn.idWorker;
                order.Employees = db.Employees.Find(AutorizWIn.idWorker);
                db.Orders.Add(order);

                foreach(var chek in cheques)
                    db.Cheques.Add(chek);

                foreach(var sost in composCheques)
                {
                    db.ComposCheques.Add(sost);
                    Product p = db.Products.Where(x=>x.Id == sost.ProductId).FirstOrDefault();
                    p.LeftInStorage -= sost.Quantity*sost.Portions;
                    db.SaveChanges();

                    Storage st = new Storage
                    {
                        EmployeeId = AutorizWIn.idWorker,
                        Quantity = sost.Quantity * sost.Portions,
                        Employees = db.Employees.Find(AutorizWIn.idWorker),
                        ProductId = p.Id,
                        Products = p,
                        Date = DateTime.Now,
                        StorageTypes = db.StorageTypes.Find(2),
                        TypeId = 1
                    };
                    db.Storage.Add(st);
                    db.SaveChanges();
                }
                
                db.SaveChanges();
                order = new Order();
                cheques.Clear();
                composCheques.Clear();
                tbTotal.Text= string.Empty;
                cheques_copy.Clear();
                cheques_copy = cheques.ToList();
                dtgOrder.ItemsSource = cheques_copy;
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            selectedType = null;
            UpDate();
        }

        private void btnQuantityNumb_Click(object sender, RoutedEventArgs e)
        {
            Cheque perem = dtgOrder.SelectedItem as Cheque;
            if (perem != null)
            {
                QuantityWin qw = new QuantityWin(perem);
                if (qw.ShowDialog() == true)
                {
                    Cheque c = cheques.Find(i => i.Id == perem.Id);

                    foreach (var i in composCheques.Where(x => x.ChequeId == c.Id))
                    {
                        i.Portions=qw.Quantity;
                    } 
                    c.Quantity = qw.Quantity;

                    cheques_copy.Clear();
                    cheques_copy = cheques.ToList();
                    dtgOrder.ItemsSource = cheques_copy;
                    tbTotal.Text = cheques.Sum(x => x.Price * x.Quantity).ToString();
                }                
            }
            else MessageBox.Show("Выберите позицию, которую хотите удалить");
        }

        private void btnQuantityMinus_Click(object sender, RoutedEventArgs e)
        {
            Cheque perem = dtgOrder.SelectedItem as Cheque;
            if (perem != null)
            {
                if (perem.Quantity > 1)
                {
                    Cheque c = cheques.Find(i=>i.Id==perem.Id);

                    foreach(var i in composCheques.Where(x=>x.ChequeId==c.Id))
                    {
                        i.Portions--;
                    }
                    c.Quantity--;
                }
                else
                {
                    cheques.Remove(cheques.Find(i => i.Id == perem.Id));
                }

                cheques_copy.Clear();
                cheques_copy = cheques.ToList();
                dtgOrder.ItemsSource = cheques_copy;
                tbTotal.Text = cheques.Sum(x => x.Price * x.Quantity).ToString();
            }
            else MessageBox.Show("Выберите позицию");
        }

        private void btnQuantityPlus_Click(object sender, RoutedEventArgs e)
        {
            List<Product> tempProd = db.Products.ToList();

            var hdj = db.Products.Join(db.ComposCheques, i => i.Id, mi => mi.ProductId, (i, mi) => new { i.Id, mi.Quantity, mi.MenuId, i.LeftInStorage, mi.ProductId, mi.Portions })
              .Select(g => new { Id = g.Id, Quantity = g.Quantity, QiS = g.LeftInStorage, prod =g.ProductId, port = g.Portions }).ToList();

            foreach (var product in tempProd)
            {
                product.LeftInStorage -= hdj.Where(x => x.prod == product.Id).Sum(s=>s.Quantity*s.port);
            }
            /////////////
            var stop = (from cc in db.ComposCheques
                        join m in db.Menu on cc.MenuId equals m.Id
                        join p in tempProd on cc.ProductId equals p.Id
                        join ch in db.Cheques on cc.ChequeId equals ch.Id
                        where cc.Quantity > p.LeftInStorage
                        select new
                        {
                            IdCheque = ch.Id,
                        }).ToList();

            Cheque c = dtgOrder.SelectedItem as Cheque;
            if (c != null)
            {
                if (stop.Any(o => o.IdCheque == c.Id) == false)
                {
                    foreach (var i in composCheques.Where(x => x.ChequeId == c.Id))
                    {
                        i.Quantity += i.Quantity / c.Quantity;
                    }

                    c.Quantity++;
                    cheques_copy.Clear();
                    cheques_copy = cheques.ToList();
                    dtgOrder.ItemsSource = cheques_copy;
                    tbTotal.Text = cheques.Sum(x => x.Price * x.Quantity).ToString();
                }
                else MessageBox.Show("Недостаточно продуктов");
            }
            else MessageBox.Show("Выберите позицию, которую хотите удалить");
        }

    }
}
