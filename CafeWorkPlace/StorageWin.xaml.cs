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
    /// Логика взаимодействия для StorageWin.xaml
    /// </summary>
    public partial class StorageWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        List<Storage> storages = new List<Storage>();
        List<Storage> storages_copy = new List<Storage>();
        public StorageWin()
        {
            InitializeComponent();

            cbProd.ItemsSource = db.Products.ToList();
            cbProd.DisplayMemberPath = "Title";

            cbProdReason.ItemsSource = MainWindow.db.StorageTypes.ToList();
            cbProdReason.DisplayMemberPath = "Title";

        }

        private void cbProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product pr = cbProd.SelectedItem as Product;
            if (pr != null)
                tbProdUnit.Text = pr.Unit;
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

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var s = (((sender as Button).DataContext) as Storage);
            storages_copy.Clear();
            storages.Remove(s);
            storages_copy = storages.ToList();
            lbStorage.ItemsSource = storages_copy;
        }

        private void btnAddReason_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.action = "Добавить";
            StorageTypeWin st = new StorageTypeWin();
            if(st.ShowDialog() == true)
            {
                cbProdReason.ItemsSource = db.StorageTypes.ToList();
                cbProdReason.DisplayMemberPath = "Title";

            }
        }

        private void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            if(f.IsDigit(tbQuant.Text) && dpProd.SelectedDate!=null && cbProdReason.SelectedIndex>= 0 && cbProd.SelectedIndex >= 0)
            {
                if (dpProd.SelectedDate <= DateTime.Now)
                {
                    Product p = cbProd.SelectedItem as Product;
                    StorageType type = cbProdReason.SelectedItem as StorageType;
                    if (p != null)
                    {
                        Storage st = new Storage
                        {
                            Date = Convert.ToDateTime(((DateTime)dpProd.SelectedDate).ToShortDateString()),
                            Quantity = Convert.ToDouble(tbQuant.Text),
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
                    else MessageBox.Show("Выберите продукт");
                }
                else MessageBox.Show("Выберите корректную дату");
            }
            else MessageBox.Show("Запоните поля");
        }
    }
}
