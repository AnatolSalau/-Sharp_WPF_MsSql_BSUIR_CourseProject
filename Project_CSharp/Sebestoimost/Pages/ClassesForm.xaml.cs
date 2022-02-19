using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class ClassesForm : Page
    {
        Class item;
        public ClassesForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Classes.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Class() { Id = 0, Measure = App.db.Measures.FirstOrDefault() };
            }
            FldMeasure.ItemsSource = App.db.Measures.ToList();
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Classes.Add(item);
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
                App.SetLogText("Ошибка сохранения номенклатурной группы\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new ClassesList());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new ClassesList());
        }
    }
}
