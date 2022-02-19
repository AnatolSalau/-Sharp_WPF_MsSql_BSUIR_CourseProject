using Sebestoimost.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class NomenclatureForm : Page
    {
        Nomenclature item;
        public NomenclatureForm(int id, int classId)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Nomenclatures.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                item = new Nomenclature() { Id = 0, NomenclatureType = App.db.NomenclatureTypes.FirstOrDefault(), Class = App.db.Classes.FirstOrDefault(p => p.Id == classId) };
            }
            FldNomenclatureType.ItemsSource = App.db.NomenclatureTypes.ToList();
            FldClass.ItemsSource = App.db.Classes.ToList();
            DataContext = item;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (item.Id == 0)
            {
                App.db.Nomenclatures.Add(item);
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
                App.SetLogText("Ошибка сохранения номенклатуры\t" + App.user.Name);
                return;
            }
            NavigationService.Navigate(new NomenclatureList(item.Class.Id));
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.db.UndoChanges();
            NavigationService.Navigate(new NomenclatureList(item.Class.Id));
        }
    }
}
