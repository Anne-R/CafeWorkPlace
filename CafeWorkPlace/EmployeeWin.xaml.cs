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
    /// Логика взаимодействия для EmployeeWin.xaml
    /// </summary>
    public partial class EmployeeWin : Window
    {
        Functions f = new Functions();
        CafeContext db = MainWindow.db;
        string temp_action;
        public EmployeeWin()
        {
            InitializeComponent();

            temp_action = MainWindow.action;

            cbPosition.ItemsSource = db.Positions.ToList();
            cbPosition.DisplayMemberPath = "Title";

            if(temp_action == "Редактировать")
            {
                Employee emp = db.Employees.Find(MainWindow.IdEmpl);
                tbxName.Text = emp.FullName;
                tbxPass.Text = emp.Password;
                cbPosition.Text = db.Positions.Find(emp.PositionId).Title;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Position p = cbPosition.SelectedItem as Position;
            if (temp_action == "Добавить")
            {
                if (!string.IsNullOrWhiteSpace(tbxName.Text) && p != null)
                {
                    if (f.IsDigit(tbxPass.Text))
                    {
                        bool rez = f.AddingEmployee(tbxName.Text, tbxPass.Text, p.Id);
                        if (rez)
                        {
                            this.DialogResult = true;
                        }
                        else MessageBox.Show("Ошибка");
                    }
                    else MessageBox.Show("Введите код цифрами");
                }
                else MessageBox.Show("Заполните поля");
            }
            else if(temp_action == "Редактировать")
            {
                if (!string.IsNullOrWhiteSpace(tbxName.Text) && p != null)
                {
                    Employee emp = db.Employees.Find(MainWindow.IdEmpl);
                    Position pos = cbPosition.SelectedItem as Position;
                    emp.FullName = tbxName.Text;
                    emp.PositionId = pos.Id;
                    emp.Positions = pos;
                    emp.Password = tbxPass.Text;
                    db.SaveChanges();
                    this.DialogResult = true;
                }
                else MessageBox.Show("Заполните поля");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult=false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.action = "Добавить";
            PositionWin pw = new PositionWin();
            if(pw.ShowDialog() == true)
            {
                cbPosition.ItemsSource = db.Positions.ToList();
                cbPosition.DisplayMemberPath = "Title";
            }
        }
    }
}
