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
    /// Логика взаимодействия для StorageTypeWin.xaml
    /// </summary>
    public partial class StorageTypeWin : Window
    {
        Functions f = new Functions();
        CafeContext db = MainWindow.db;
        public StorageTypeWin()
        {
            InitializeComponent();

            if (MainWindow.action == "Редактировать")
            {
                StorageType st = db.StorageTypes.Find(MainWindow.IdStorageType);
                tbxTitle.Text = st.Title;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxTitle.Text))
            {
                if (MainWindow.action == "Добавить")
                {
                    bool rez = f.AddingStorageType(tbxTitle.Text);
                    if (rez)
                    {
                        this.DialogResult = true;
                    }
                    else MessageBox.Show("Ошибка");
                }
                else if (MainWindow.action == "Редактировать")
                {
                    StorageType st = db.StorageTypes.Find(MainWindow.IdStorageType);
                    st.Title = tbxTitle.Text;
                    db.SaveChanges();
                    this.DialogResult=true;
                }
            }
            else MessageBox.Show("Заполните поля");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
