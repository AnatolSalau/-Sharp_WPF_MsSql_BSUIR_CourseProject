using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class DepartmentsForm : Page
    {
        Department item;
        public DepartmentsForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Departments.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Department() { Id = 0 };
            }
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Departments.Add(item);
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
                App.SetLogText("Ошибка сохранения подразделения\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new DepartmentsList());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new DepartmentsList());
        }
    }
}
