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
    /// Логика взаимодействия для MenuTypeWin.xaml
    /// </summary>
    public partial class MenuTypeWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        public MenuTypeWin()
        {
            InitializeComponent();

            MenuType mt = db.MenuTypes.Find(MainWindow.IdMenuType);

            if (MainWindow.action == "Редактировать")
            {
                tbxTitle.Text = mt.Title;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxTitle.Text))
            {
                if (MainWindow.action == "Добавить")
                {
                    bool rez = f.AddingMenuType(tbxTitle.Text);
                    if (rez)
                    {
                        this.DialogResult = true;
                        
                    }
                }
                else if (MainWindow.action == "Редактировать")
                {
                    MenuType mt = db.MenuTypes.Find(MainWindow.IdMenuType);
                    mt.Title = tbxTitle.Text;
                    db.SaveChanges();
                    this.DialogResult = true;
                    
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult=false;
            
        }
    }
}
