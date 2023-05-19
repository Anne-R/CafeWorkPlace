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
    /// Логика взаимодействия для EditMenuWin.xaml
    /// </summary>
    public partial class EditMenuWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        Menu m = MainWindow.db.Menu.Find(MainWindow.idMenu);

        List<Product> Allproducts = new List<Product>();
        List<Product> Allprod_copy = new List<Product>();
        List<Composition> compositions = new List<Composition>();
        List<Composition> comp_copy = new List<Composition>();

        public EditMenuWin()
        {
            InitializeComponent();

            cbCategory.ItemsSource = db.MenuTypes.ToList();
            cbCategory.SelectedItem = db.MenuTypes.Find(m.TypeId);

            tbPrice.Text = m.Price.ToString();
            tbSize.Text = m.Size.ToString();
            tbTitle.Text = m.Title.ToString();

            if(m.Unit=="гр")
                cbUnit.SelectedIndex = 0;
            else if (m.Unit == "мл")  
                cbUnit.SelectedIndex = 1;

            compositions = db.Compositions.Where(x=>x.MenuId==m.Id).ToList();
            lbComposition.ItemsSource = compositions;

            Allproducts = db.Products.ToList();
            foreach(Composition c in compositions)
            {
                Allproducts.Remove(Allproducts.Where(x=>x.Id == c.ProductId).First());
            }
            cbProduct.ItemsSource = Allproducts;
            cbProduct.DisplayMemberPath = "Title";
        }

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product pr = cbProduct.SelectedItem as Product;
            if (pr != null)
                tbProdUnit.Text = pr.Unit;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product p = cbProduct.SelectedItem as Product;
            if (p != null && f.IsDigit(tbQuant.Text))
            {
                Composition com = new Composition
                {
                    MenuId = m.Id,
                    ProductId = p.Id,
                    Menu = db.Menu.Find(m.Id),
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

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbTitle.Text) && !string.IsNullOrWhiteSpace(tbSize.Text)
                && cbUnit.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(tbPrice.Text) && cbCategory.SelectedIndex >= 0)
            {
                if (f.IsDigit(tbPrice.Text) && f.IsDigit(tbSize.Text))
                {
                    m.Title = tbTitle.Text;
                    m.Unit = cbUnit.Text;
                    m.Price = Convert.ToDouble(tbPrice.Text);
                    m.Size = Convert.ToDouble(tbSize.Text);
                    MenuType mt = cbCategory.SelectedItem as MenuType;
                    m.TypeId = mt.Id;
                    m.MenuTypes = mt;

                    if (compositions.Count() >= 1)
                    {
                        foreach (Composition c in db.Compositions.Where(x => x.MenuId == m.Id).ToList())
                            db.Compositions.Remove(c);

                        db.SaveChanges();

                        foreach (var item in compositions)
                            db.Compositions.Add(item);
                        db.SaveChanges();
                        this.DialogResult = true;
                    }
                    else MessageBox.Show("Добавьте состав");
                }
                else MessageBox.Show("Введите значение цифрами");
            }
            else MessageBox.Show("Заполните поля");            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.action = "Добавить";
            MenuTypeWin menuTypeWin = new MenuTypeWin();
            if (menuTypeWin.ShowDialog() == true)
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
