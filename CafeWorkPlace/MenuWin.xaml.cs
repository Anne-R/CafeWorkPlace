using CafeWorkPlace.db;
using System;
using System.Collections;
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
using Menu = CafeWorkPlace.db.Menu;

namespace CafeWorkPlace
{
    /// <summary>
    /// Логика взаимодействия для MenuWin.xaml
    /// </summary>
    public partial class MenuWin : Window
    {
        List<Product>  Allproducts = new List<Product>();
        List<Product> Allprod_copy = new List<Product>();
        List<Composition> compositions = new List<Composition>();
        List<Composition> comp_copy = new List<Composition>();
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        int idMenu;
        bool flag = false;
        public MenuWin()
        {
            InitializeComponent();

            Allproducts = db.Products.ToList();
            cbProduct.ItemsSource = Allproducts;
            cbProduct.DisplayMemberPath = "Title";
            cbCategory.ItemsSource = db.MenuTypes.ToList();
            cbCategory.DisplayMemberPath = "Title";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(tbTitle.Text) && !string.IsNullOrWhiteSpace(tbSize.Text) 
                && cbUnit.SelectedIndex>=0 && !string.IsNullOrWhiteSpace(tbPrice.Text) && cbCategory.SelectedIndex >= 0)
            {
                if (f.IsDigit(tbPrice.Text) && f.IsDigit(tbSize.Text))
                {
                    if(!db.Menu.Any(x=>x.Title == tbTitle.Text))
                    {
                        MenuType mc = cbCategory.SelectedItem as MenuType;
                        Menu m = new Menu
                        {
                            Title = tbTitle.Text,
                            Size = Convert.ToDouble(tbSize.Text),
                            Price = Convert.ToDouble(tbPrice.Text),
                            TypeId = mc.Id,
                            MenuTypes = mc,
                            Unit = cbUnit.Text,
                            IsActive = true
                        };
                        db.Menu.Add(m);
                        db.SaveChanges();
                        idMenu = m.Id;
                        flag = true;
                        spSostav.IsEnabled = true;
                    }
                    else MessageBox.Show("Товар с таким наименованием уже существует");
                }
                else MessageBox.Show("Введите значение цифрами");
            }
            else MessageBox.Show("Заполните поля");
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product p = cbProduct.SelectedItem as Product;
            if (p != null && f.IsDigit(tbQuant.Text))
            {
                Composition com = new Composition
                {
                    MenuId = idMenu,
                    ProductId = p.Id,
                    Menu = db.Menu.Find(idMenu),
                    Products = db.Products.Find(p.Id),
                    Quantity = Convert.ToDouble(tbQuant.Text)
                };

                comp_copy.Clear();
                compositions.Add(com);
                comp_copy = compositions.ToList();

                lbComposition.ItemsSource = comp_copy;

                Allprod_copy.Clear();
                Allproducts.Remove(p);
                Allprod_copy = Allproducts.ToList();
                cbProduct.ItemsSource = Allprod_copy;
                cbProduct.DisplayMemberPath = "Title";
                tbQuant.Text = "";
                tbProdUnit.Text = "";
            }
            else MessageBox.Show("Выберите продукт и введите количество цифрами");
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (compositions.Count() >= 1)
            {
                foreach (var item in compositions)
                    db.Compositions.Add(item);
                db.SaveChanges();
                this.DialogResult = true;
            }
            else MessageBox.Show("Добавьте состав");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                db.Menu.Remove(db.Menu.Find(idMenu));
                db.SaveChanges();
            }
            this.DialogResult = false;
        }

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                Product pr = cbProduct.SelectedItem as Product;
            if (pr!=null)
                tbProdUnit.Text = pr.Unit;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var s = (((sender as Button).DataContext) as Composition);
            comp_copy.Clear();
            compositions.Remove(s);
            comp_copy = compositions.ToList();
            lbComposition.ItemsSource = comp_copy;
            Product p = db.Products.Find(s.ProductId);
            Allproducts.Add(p);
            Allprod_copy.Clear();
            Allprod_copy = Allproducts.ToList();
            cbProduct.ItemsSource = Allprod_copy;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.action = "Добавить";
            MenuTypeWin menuTypeWin = new MenuTypeWin();
            if(menuTypeWin.ShowDialog()==true)
            {
                cbCategory.ItemsSource = db.MenuTypes.ToList();
                cbCategory.DisplayMemberPath = "Title";
            }
        }

        private void btnNewProd_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.action = "Добавить";
            ProductWin pw = new ProductWin();
            if (pw.ShowDialog() == true)
            {
                cbProduct.ItemsSource = db.Products.ToList();
                cbProduct.DisplayMemberPath = "Title";
            }
        }
    }
}
