using CafeWorkPlace.db;
using CafeWorkPlace.KassWindows;
using System;
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

namespace CafeWorkPlace
{
    /// <summary>
    /// Логика взаимодействия для AutorizWIn.xaml
    /// </summary>
    public partial class AutorizWIn : Window
    {
        CafeContext db = new CafeContext();
        public static int idWorker;
        public AutorizWIn()
        {
            InitializeComponent();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "7";
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "8";
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "9";
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "4";
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "5";
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "6";
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "1";
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "2";
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "3";
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            tbPass.Password += "0";
        }

        private void btnErase_Click(object sender, RoutedEventArgs e)
        {
            string str = tbPass.Password;
            if(str.Length > 0)
                tbPass.Password = str.Remove((str.Length - 1), 1);
        }

        private void btnKass_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbPass.Password))
            {
                if(db.Employees.Any(x=>x.Password== tbPass.Password))
                {
                    foreach (var user in db.Employees)
                    {
                        if (user.Password == tbPass.Password)
                        {
                            idWorker = user.Id;

                            KassWin aw = new KassWin();
                            aw.Show();
                            this.Close();
                        }

                    }
                }
                else MessageBox.Show("Неверный пароль");

            }
            else MessageBox.Show("Введите пароль");
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbPass.Password))
            {
                if (db.Employees.Any(x => x.Password == tbPass.Password && x.IsAdmin))
                {
                    foreach (var user in db.Employees)
                    {
                        if (user.Password == tbPass.Password)
                        {
                            if (user.IsAdmin)
                            {
                                idWorker = user.Id;
                                MainWindow aw = new MainWindow();
                                aw.Show();
                                this.Close();
                            }
                        }
                    }
                }
                else MessageBox.Show("Неверный пароль");
            }
            else MessageBox.Show("Введите пароль");
        }
    }
}
