using CafeWorkPlace.db;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Menu = CafeWorkPlace.db.Menu;

namespace CafeWorkPlace
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static CafeContext db = new CafeContext();
        public static string action;
        public static int IdPos;
        public static int IdEmpl;
        public static int IdProdType;
        public static int IdProd;
        public static int IdStorageType;
        public static int idStorage;
        public static int IdMenuType;
        public static int idMenu;

        public MainWindow()
        {
            InitializeComponent();

            AutorizWIn.idWorker = 1;

            dtgPositions.ItemsSource = db.Positions.ToList();
            dtgWorkers.ItemsSource = db.Employees.ToList();
            //lbProductType.ItemsSource = db.ProductTypes.ToList();

            lbProductType.ItemsSource = db.ProductTypes.Local.ToBindingList();

            dtgProducts.ItemsSource = db.Products.ToList();
            dtgStorage.ItemsSource = db.Storage.ToList();
            dtgStorageTypes.ItemsSource = db.StorageTypes.ToList();
            dtgMenu.ItemsSource = db.Menu.Where(x=>x.IsActive).ToList();
            dtgMenuTypes.ItemsSource= db.MenuTypes.ToList();

            /*MenuType mt1 = new MenuType
            {
                Title = "Десерт"
            };
            db.MenuTypes.Add(mt1);
            db.SaveChanges();*/
        }

        private void UpDate()
        {
            if(cb.IsChecked == true)
                dtgMenu.ItemsSource = db.Menu.ToList();
            else dtgMenu.ItemsSource = db.Menu.Where(x => x.IsActive).ToList();
        }

        private void btnAddPosition_Click(object sender, RoutedEventArgs e)
        {
            action = "Добавить";
            PositionWin pw = new PositionWin();
            if (pw.ShowDialog() == true)
            {
                MessageBox.Show("Позиция добавлена");
            }
            List<Position> positions = db.Positions.ToList();
            dtgPositions.ItemsSource = positions;
        }

        private void btnAddSotrud_Click(object sender, RoutedEventArgs e)
        {
            action = "Добавить";
            EmployeeWin ew = new EmployeeWin();
            if (ew.ShowDialog() == true)
            {
                MessageBox.Show("Сотрудник добавлен");
            }
            dtgWorkers.ItemsSource = db.Employees.ToList();
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            AutorizWIn aw = new AutorizWIn();
            aw.Show();
            this.Close();
        }

        private void btnEditPos_Click(object sender, RoutedEventArgs e)
        {
            action = "Редактировать";
            Position p = dtgPositions.SelectedItem as Position;
            if(p != null )
            {
                IdPos = p.Id;
                PositionWin pw = new PositionWin();
                if (pw.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                dtgPositions.ItemsSource = db.Positions.ToList();
            }
            else MessageBox.Show("Выберите позицию");
        }

        private void btnEditemp_Click(object sender, RoutedEventArgs e)
        {
            action = "Редактировать";
            Employee emp = dtgWorkers.SelectedItem as Employee;
            if (emp != null)
            {
                IdEmpl = emp.Id;
                EmployeeWin ew = new EmployeeWin();
                if (ew.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                dtgWorkers.ItemsSource = db.Employees.ToList();
            }
            else MessageBox.Show("Выберите сотрудника");
        }

        private void btnEditPType_Click(object sender, RoutedEventArgs e)
        {
            action = "Редактировать";
            ProductType pt = lbProductType.SelectedItem as ProductType;
            if (pt != null)
            {
                IdProdType = pt.Id;
                ProdTypeWin prodTypeWin = new ProdTypeWin();
                if(prodTypeWin.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                lbProductType.Items.Refresh();
                //ObservableCollection<ProductType> products = new ObservableCollection<ProductType>(db.ProductTypes.ToList());
                //lbProductType.ItemsSource = products;
                //dtgProductType.Items.Refresh();
            }
        }

        private void btnEditProd_Click(object sender, RoutedEventArgs e)
        {
            action = "Редактировать";

            Product p = dtgProducts .SelectedItem as Product;
            if(p != null)
            {
                IdProd = p.Id;
                ProductWin prodWin = new ProductWin();
                if (prodWin.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                dtgProducts.ItemsSource = db.Products.ToList();
            }
            else MessageBox.Show("Выберите продукт");
        }

        private void btnAddProdType_Click(object sender, RoutedEventArgs e)
        {
            action = "Добавить";
            ProdTypeWin prodWin = new ProdTypeWin();
            if (prodWin.ShowDialog() == true)
            {
                MessageBox.Show("Тип добавлен");
            }
            lbProductType.ItemsSource = db.ProductTypes.ToList();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            action = "Добавить";
            ProductWin prodWin = new ProductWin();
            if(!prodWin.ShowDialog() == true)
            {
                MessageBox.Show("Продукт добавлен");
            }
            dtgProducts.ItemsSource = db.Products.ToList();
        }

        private void btnAddPostavka_Click(object sender, RoutedEventArgs e)
        {
            StorageWin aw = new StorageWin();
            if (aw.ShowDialog() == true)
            {
            }
            dtgStorage.ItemsSource = db.Storage.ToList();
        }

        private void btnMinusProd_Click(object sender, RoutedEventArgs e)
        {
            SpisanieWin aw = new SpisanieWin();
            if (aw.ShowDialog() == true)
            {
            }
            dtgStorage.ItemsSource = db.Storage.ToList();
        }

        private void btnEditSTypes_Click(object sender, RoutedEventArgs e)
        {
            action = "Редактировать";
            StorageType st = dtgStorageTypes.SelectedItem as StorageType;
            if (st != null)
            {
                IdStorageType = st.Id;
                StorageTypeWin stw = new StorageTypeWin();
                if (stw.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                dtgStorageTypes.ItemsSource = db.StorageTypes.ToList();
            }
        }

        private void btnAddType_Click(object sender, RoutedEventArgs e)
        {
            action = "Добавить";
            StorageTypeWin stw = new StorageTypeWin();

            if(stw.ShowDialog() == true)
            {
                MessageBox.Show("Категория добавлена");
            }
            dtgStorageTypes.ItemsSource = db.StorageTypes.ToList();
        }

        private void btnAddMenuType_Click(object sender, RoutedEventArgs e)
        {
            action = "Добавить";
            MenuTypeWin mtw = new MenuTypeWin();
            if (mtw.ShowDialog() == true)
            {
                MessageBox.Show("Категория добавлена");
            }
            dtgMenuTypes.ItemsSource = db.MenuTypes.ToList();

        }

        private void btnEditSMenuTypes_Click(object sender, RoutedEventArgs e)
        {
            action = "Редактировать";
            MenuType mt = dtgMenuTypes.SelectedItem as MenuType;
            if (mt != null)
            {
                IdMenuType = mt.Id;
                MenuTypeWin menuTypeWin = new MenuTypeWin();
                if(menuTypeWin.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                dtgMenuTypes.ItemsSource = db.MenuTypes.ToList();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpDate();
        }


        private void btnEditMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu m = dtgMenu.SelectedItem as Menu;
            if (m != null)
            {
                idMenu = m.Id;
                EditMenuWin ew = new EditMenuWin();
                if (ew.ShowDialog() == true)
                {
                    MessageBox.Show("Изменения сохранены");
                }
                UpDate();
            }            
        }

        private void btnAddMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuWin menuWin = new MenuWin();
            if (menuWin.ShowDialog() == true)
            {
                MessageBox.Show("Товар добавлен");
            }
            UpDate();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpDate();
        }

        private void dtgMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Menu menu = dtgMenu.SelectedItem as Menu;
            if (menu != null)
            {
                if(menu.IsActive)
                {
                    btnActivate.Visibility = Visibility.Collapsed;
                    btnDisActivate.Visibility = Visibility.Visible;
                }
                else
                {
                    btnActivate.Visibility = Visibility.Visible;
                    btnDisActivate.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = dtgMenu.SelectedItem as Menu;
            if (menu != null)
            {
               menu.IsActive = true;
               db.SaveChanges();
               UpDate();
            }
        }

        private void btnDisActivate_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = dtgMenu.SelectedItem as Menu;
            if (menu != null)
            {
                menu.IsActive = false;
                db.SaveChanges();
                UpDate();
            }
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            Employee epm = dtgWorkers.SelectedItem as Employee;
            if (epm != null)
            {
                epm.IsAdmin = true;
                db.SaveChanges();
            }
        }

        private void btnNotAdmin_Click(object sender, RoutedEventArgs e)
        {
            Employee epm = dtgWorkers.SelectedItem as Employee;
            if (epm != null)
            {
                epm.IsAdmin = false;
                db.SaveChanges();
            }
        }

        private void dtgWorkers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee epm = dtgWorkers.SelectedItem as Employee;
            if (epm != null)
            {
                if (epm.IsAdmin)
                {
                    btnAdmin.Visibility = Visibility.Collapsed;
                    btnNotAdmin.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAdmin.Visibility = Visibility.Visible;
                    btnNotAdmin.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
