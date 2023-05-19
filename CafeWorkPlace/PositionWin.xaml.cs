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
    /// Логика взаимодействия для PositionWin.xaml
    /// </summary>
    public partial class PositionWin : Window
    {
        CafeContext db = MainWindow.db;
        Functions f = new Functions();
        public PositionWin()
        {
            InitializeComponent();

            if (MainWindow.action == "Редактировать")
            {
                Position p = db.Positions.Find(MainWindow.IdPos);
                tbxTitle.Text = p.Title;
                tbxSalary.Text = Convert.ToString(p.Salary);
                tbxBonus.Text = Convert.ToString(p.SalesBonus);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(MainWindow.action == "Добавить")
            {
                if (!string.IsNullOrWhiteSpace(tbxSalary.Text) 
                    && !string.IsNullOrWhiteSpace(tbxBonus.Text) && !string.IsNullOrWhiteSpace(tbxTitle.Text))
                {
                    if (f.IsDigit(tbxSalary.Text) && f.IsDigit(tbxBonus.Text))
                    {
                        bool rez = f.AddingPosition(tbxTitle.Text, tbxSalary.Text, tbxBonus.Text);
                        if(rez)
                            this.DialogResult = true;
                        else MessageBox.Show("Ошибка");
                    }
                    else MessageBox.Show("Введите значения цифрами");
                }
                else MessageBox.Show("Заполните поля");
            }
            else if(MainWindow.action == "Редактировать")
            {
                if (!string.IsNullOrWhiteSpace(tbxSalary.Text)
                    && !string.IsNullOrWhiteSpace(tbxBonus.Text) && !string.IsNullOrWhiteSpace(tbxTitle.Text))
                {
                    if (f.IsDigit(tbxSalary.Text) && f.IsDigit(tbxBonus.Text))
                    {
                        Position p = db.Positions.Find(MainWindow.IdPos);
                        p.Title = tbxTitle.Text;
                        p.Salary = Convert.ToDouble(tbxSalary.Text);
                        p.SalesBonus = Convert.ToDouble(tbxBonus.Text);
                        db.SaveChanges();
                        this.DialogResult = true;
                    }
                    else MessageBox.Show("Введите значения цифрами");
                }
                else MessageBox.Show("Заполните поля");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
