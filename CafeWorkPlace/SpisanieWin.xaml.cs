using CafeWorkPlace.db;
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
using Menu = CafeWorkPlace.db.Menu;

namespace CafeWorkPlace
{
    /// <summary>
    /// Логика взаимодействия для SpisanieWin.xaml
    /// </summary>
    public partial class SpisanieWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        List<Storage> storages = new List<Storage>();
        List<Storage> storages_copy = new List<Storage>();
        List<Composition> compositions = new List<Composition>();
        public SpisanieWin()
        {
            InitializeComponent();

            cbProd.ItemsSource = db.Products.ToList();
            cbProd.DisplayMemberPath = "Title";

            cbProdReason.ItemsSource = db.StorageTypes.ToList();
            cbProdReason.DisplayMemberPath = "Title";

            cbtovar.ItemsSource = db.Menu.ToList();
            cbtovar.DisplayMemberPath = "Title";

            cbTovarReason.ItemsSource = db.StorageTypes.ToList();
            cbTovarReason.DisplayMemberPath = "Title";

        }

        private void rbProd_Checked(object sender, RoutedEventArgs e)
        {
            stTovar.IsEnabled = false;
            stProd.IsEnabled = true;
        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product pr = cbProd.SelectedItem as Product;
            if (pr != null)
                tbProdUnit.Text = pr.Unit;
        }

        private void btnAddReason_Click(object sender, RoutedEventArgs e)
        {
            StorageTypeWin st = new StorageTypeWin();
            if(st.ShowDialog() == true)
            {
                cbProdReason.ItemsSource = db.StorageTypes.ToList();
                cbProdReason.DisplayMemberPath = "Title";

                cbTovarReason.ItemsSource = db.StorageTypes.ToList();
                cbTovarReason.DisplayMemberPath = "Title";
            }
        }

        private void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            if (f.IsDigit(tbQuant.Text) && dpProd.SelectedDate != null && cbProdReason.SelectedIndex >= 0 && cbProd.SelectedIndex >= 0)
            {
                if (dpProd.SelectedDate <= DateTime.Now)
                {
                    Product p = cbProd.SelectedItem as Product;
                    StorageType type = cbProdReason.SelectedItem as StorageType;
                    if (p != null)
                    {
                        if(p.LeftInStorage>= Convert.ToDouble(tbQuant.Text))
                        {
                            Storage st = new Storage
                            {
                                Date = Convert.ToDateTime(((DateTime)dpProd.SelectedDate).ToShortDateString()),
                                Quantity = -1 * Convert.ToDouble(tbQuant.Text),
                                TypeId = type.Id,
                                StorageTypes = type,
                                EmployeeId = AutorizWIn.idWorker,
                                Employees = db.Employees.Find(AutorizWIn.idWorker),
                                ProductId = p.Id,
                                Products = p
                            };
                            storages.Add(st);
                            storages_copy.Clear();
                            storages_copy = storages.ToList();
                            lbStorage.ItemsSource = storages_copy;
                        }
                        else MessageBox.Show("Невозможно списать больше имеющегося на складе");
                    }
                    else MessageBox.Show("Выберите продукт");
                }
                else MessageBox.Show("Выберите корректную дату");
            }
            else MessageBox.Show("Запоните поля");
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (storages.Count > 0)
            {
                foreach (var item in storages)
                {
                    db.Storage.Add(item);
                    Product p = db.Products.Find(item.ProductId);
                    p.LeftInStorage += item.Quantity;
                }

                db.SaveChanges();
                this.DialogResult = true;
            }
            else MessageBox.Show("Выберите продукт");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void rbTovar_Checked(object sender, RoutedEventArgs e)
        {
            stTovar.IsEnabled = true;
            stProd.IsEnabled = false;
        }

        private void btnAddTovar_Click(object sender, RoutedEventArgs e)
        {
            if (f.IsDigit(tbTovarQuant.Text) && dpTovar.SelectedDate != null && cbTovarReason.SelectedIndex >= 0 && cbtovar.SelectedIndex >= 0)
            {
                if (dpTovar.SelectedDate <= DateTime.Now)
                {
                    Menu m = cbtovar.SelectedItem as Menu;
                    StorageType type = cbTovarReason.SelectedItem as StorageType;
                    if (m != null && type!=null)
                    {
                        compositions = db.Compositions.Where(x => x.MenuId == m.Id).ToList();
                        foreach (Composition item in compositions)
                        {
                            Product p = db.Products.Find(item.ProductId);
                            Storage st = new Storage
                            {
                                Date = Convert.ToDateTime(((DateTime)dpTovar.SelectedDate).ToShortDateString()),
                                Quantity = item.Quantity*(-1),
                                TypeId = type.Id,
                                StorageTypes = type,
                                EmployeeId = AutorizWIn.idWorker,
                                Employees = db.Employees.Find(AutorizWIn.idWorker),
                                ProductId = p.Id,
                                Products = p
                            };
                            storages.Add(st);
                        }
                        
                        storages_copy.Clear();
                        storages_copy = storages.ToList();
                        lbStorage.ItemsSource = storages_copy;
                    }
                    else MessageBox.Show("Выберите продукт");
                }
                else MessageBox.Show("Выберите корректную дату");
            }
            else MessageBox.Show("Запоните поля");
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var s = (((sender as Button).DataContext) as Storage);
            storages_copy.Clear();
            storages.Remove(s);
            storages_copy = storages.ToList();
            lbStorage.ItemsSource = storages_copy;
        }
    }
}
