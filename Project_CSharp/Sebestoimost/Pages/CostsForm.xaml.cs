using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sebestoimost.Model;

namespace Sebestoimost.Pages
{
    public partial class CostsForm : Page
    {
        Cost item;
        public CostsForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Costs.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Cost() { Id = 0, Date = DateTime.Now, User = App.db.Users.First(p => p.Id == App.user.Id) };
            }
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Costs.Add(item);
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
                App.SetLogText("Ошибка сохранения расчета себестоимости\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new CostsCalc(item.Id));
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new CostsList());
        }
    }
}
