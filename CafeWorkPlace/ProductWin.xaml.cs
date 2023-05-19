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

namespace CafeWorkPlace
{
    /// <summary>
    /// Логика взаимодействия для ProductWin.xaml
    /// </summary>
    public partial class ProductWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        string temp_action;
        public ProductWin()
        {
            InitializeComponent();
            temp_action = MainWindow.action;
            cbType.ItemsSource = db.ProductTypes.ToList();
            cbType.DisplayMemberPath = "Title";

            if (temp_action == "Редактировать")
            {
                Product product = db.Products.Find(MainWindow.IdProd);
                tbxTitle.Text = product.Title;
                cbType.SelectedItem = product.ProductTypes;
                tbxPortionSize.Text = product.PortionSize.ToString();
                tbxPortionPrice.Text = product.PortionPrice.ToString();

                switch (product.Unit)
                {
                    case "шт":
                        cbUnit.SelectedIndex = 0;
                        break;
                    case "л":
                        cbUnit.SelectedIndex = 1;
                        break;
                    case "кг":
                        cbUnit.SelectedIndex = 2;
                        break;
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (cbType.SelectedIndex >= 0 && !string.IsNullOrWhiteSpace(tbxTitle.Text) && cbUnit.SelectedIndex >= 0
                && f.IsDigit(tbxPortionPrice.Text) && f.IsDigit(tbxPortionSize.Text))
            {
                if (temp_action == "Редактировать")
                {
                    Product product = db.Products.Find(MainWindow.IdProd);
                    ProductType productType = cbType.SelectedItem as ProductType;
                    product.Title = tbxTitle.Text;
                    product.Unit = cbUnit.Text;
                    product.ProductTypes = productType;
                    product.TypeId = product.TypeId;
                    product.PortionPrice = Convert.ToDouble(tbxPortionPrice.Text);
                    product.PortionSize = Convert.ToDouble(tbxPortionSize.Text);
                    db.SaveChanges();
                    this.DialogResult = true;
                }
                else if (temp_action == "Добавить")
                {
                    ProductType productType = cbType.SelectedItem as ProductType;
                    if(db.Products.Where(x=>x.Title ==  tbxTitle.Text).Any())
                        MessageBox.Show("Продукт с таким названием уже существует");
                    else
                    {
                        bool rez = f.AddingProduct(tbxTitle.Text, cbUnit.Text, productType.Id, Convert.ToDouble(tbxPortionSize.Text), Convert.ToDouble(tbxPortionPrice.Text));
                        if (rez)
                        {
                            this.DialogResult = true;
                        }
                        else MessageBox.Show("Ошибка");
                    }                   
                }
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
            ProdTypeWin prodTypeWin = new ProdTypeWin();
            if (prodTypeWin.ShowDialog() == true)
            {
                cbType.ItemsSource = db.ProductTypes.ToList();
                cbType.DisplayMemberPath = "Title";
            }
        }
    }
}
