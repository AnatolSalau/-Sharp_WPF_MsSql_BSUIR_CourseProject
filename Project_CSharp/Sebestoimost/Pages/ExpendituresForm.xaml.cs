using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class ExpendituresForm : Page
    {
        Expenditure item;
        public ExpendituresForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Expenditures.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Expenditure() { Id = 0 };
            }
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Expenditures.Add(item);
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
                App.SetLogText("Ошибка сохранения статьи затрат\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new ExpendituresList());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new ExpendituresList());
        }
    }
}
