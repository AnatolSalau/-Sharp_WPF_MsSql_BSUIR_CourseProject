using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class PlansForm : Page
    {
        Plan item;
        public PlansForm(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Plans.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Plan()
                {
                    Id = 0,
                    Date = DateTime.Now,
                    Nomenclature = App.db.Nomenclatures.FirstOrDefault(p => p.NomenclatureTypeId == 1),
                    Price = 1
                };
            }
            FldNomenclature.ItemsSource = App.db.Nomenclatures.Where(p => p.NomenclatureTypeId == 1).ToList();
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Plans.Add(item);
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
                App.SetLogText("Ошибка сохранения плановой цены\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new PlansList());
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new PlansList());
        }
    }
}
