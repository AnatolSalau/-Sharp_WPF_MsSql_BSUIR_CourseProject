using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class MeasuresForm : Page
    {
        Measure item;
        public MeasuresForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Measures.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Measure() { Id = 0 };
            }
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Measures.Add(item);
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
                App.SetLogText("Ошибка сохранения единицы измерения\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new MeasuresList());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new MeasuresList());
        }
    }
}
