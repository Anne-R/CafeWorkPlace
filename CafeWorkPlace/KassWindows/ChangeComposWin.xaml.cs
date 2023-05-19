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

namespace CafeWorkPlace.KassWindows
{
    /// <summary>
    /// Логика взаимодействия для ChangeComposWin.xaml
    /// </summary>
    public partial class ChangeComposWin : Window
    {
        CafeContext db = KassWin.db;
        Cheque cheque = new Cheque();
        List<ComposCheque> temp = new List<ComposCheque>();
        List<ComposCheque> tempCopy = new List<ComposCheque>();
        double money = 0;
        public ChangeComposWin(Cheque c, List<ComposCheque> sost)
        {
            InitializeComponent();

            temp.Clear();
            tempCopy.Clear();
            cheque = c;
            tbPrice.Text = c.Price.ToString();
            tbTitle.Text = db.Menu.Find(c.MenuId).Title;
            cbProduct.ItemsSource = db.Products.ToList();
            cbProduct.DisplayMemberPath = "Title";
            tbComm.Text = c.Comment;
            temp = sost.Where(x => x.MenuId == c.MenuId && x.ChequeId == c.Id).ToList();
            lbComposition.ItemsSource = temp;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Product product = cbProduct.SelectedItem as Product;
            if (product != null)
            {
                ComposCheque com = new ComposCheque
                {
                    ChequeId = cheque.Id,
                    Cheques = cheque,
                    ProductId = product.Id,
                    Products = product,
                    Quantity = product.PortionSize,
                    Portions = cheque.Quantity,
                    MenuId = cheque.MenuId,
                    Menu = db.Menu.Find(cheque.MenuId)
                };
                money += product.PortionPrice;
                tempCopy.Clear();
                temp.Add(com);
                tempCopy = temp.ToList();
                lbComposition.ItemsSource = tempCopy;

                tbPrice.Text = (cheque.Price + money).ToString();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var s = (((sender as Button).DataContext) as ComposCheque);
            tempCopy.Clear();
            temp.Remove(s);
            tempCopy = temp.ToList();
            money -= (db.Products.Where(x => x.Id == s.ProductId).FirstOrDefault()).PortionPrice;
            tbPrice.Text = (cheque.Price + money).ToString();
            lbComposition.ItemsSource = tempCopy;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (temp.Count() > 0)
            {
                if (!string.IsNullOrWhiteSpace(tbComm.Text))
                    cheque.Comment = tbComm.Text;
                else cheque.Comment = "изм";

                cheque.Price += money;
                this.DialogResult = true;
            }
            else MessageBox.Show("Заполните состав");
        }

        public List<ComposCheque> lst
        {
            get { return temp; }
        }
        public Cheque ch
        {
            get { return cheque; }
        }
    }
}
