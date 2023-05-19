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
    /// Логика взаимодействия для ProdTypeWin.xaml
    /// </summary>
    public partial class ProdTypeWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f= new Functions();
        public ProdTypeWin()
        {
            InitializeComponent();

            if (MainWindow.action == "Редактировать")
            {
                ProductType pr = db.ProductTypes.Find(MainWindow.IdProdType);
                tbxTitle.Text = pr.Title;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxTitle.Text))
            {
                if (MainWindow.action == "Редактировать")
                {
                    ProductType pr = db.ProductTypes.Find(MainWindow.IdProdType);
                    pr.Title = tbxTitle.Text;
                    db.SaveChanges();
                    this.DialogResult = true;
                }
                else if (MainWindow.action == "Добавить")
                {
                    bool rez = f.AddingProdType(tbxTitle.Text);
                    if (rez)
                    {
                        this.DialogResult = true;
                    }
                    else MessageBox.Show("Ошибка");
                }
            }
            else MessageBox.Show("Заполните поле");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
