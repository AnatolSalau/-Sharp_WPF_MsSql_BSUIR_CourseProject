using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class ExpensesForm : Page
    {
        Expense item;
        public ExpensesForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Expenses.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Expense()
                {
                    Id = 0,
                    Date = DateTime.Now,
                    Nomenclature = App.db.Nomenclatures.FirstOrDefault(p => p.NomenclatureTypeId == 2),
                    Summa = 1,
                    Expenditure = App.db.Expenditures.FirstOrDefault(),
                    Department = App.db.Departments.FirstOrDefault(),
                    Class = App.db.Classes.FirstOrDefault()
                };
            }
            FldNomenclature.ItemsSource = App.db.Nomenclatures.Where(p => p.NomenclatureTypeId == 2).ToList();
            FldExpenditure.ItemsSource = App.db.Expenditures.ToList();
            FldDepartment.ItemsSource = App.db.Departments.ToList();
            FldClass.ItemsSource = App.db.Classes.ToList();
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Expenses.Add(item);
            }
            else
            {
                App.db.Entry(item).State = EntityState.Modified;
            }
            try
            {
                App.db.SaveChanges();
            }
            catch (Exception ex)
            {
                App.db.UndoChanges();
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                App.SetLogText("Ошибка сохранения документа затрат\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new ExpensesList());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new ExpensesList());
        }
    }
}
