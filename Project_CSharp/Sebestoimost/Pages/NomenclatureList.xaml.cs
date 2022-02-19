using Sebestoimost.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class NomenclatureList : Page
    {
        Class parent;
        public NomenclatureList(int classId)
        {
            InitializeComponent();
            App.db = new dbContext();
            parent = App.db.Classes.FirstOrDefault(p => p.Id == classId);
            if (parent == null) parent = App.db.Classes.FirstOrDefault();
            LstParents.ItemsSource = App.db.Classes.ToList();
            LstParents.SelectedItem = parent;
        }

        private void LstParents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstParents.SelectedItem != null)
            {
                parent = LstParents.SelectedItem as Class;
                GrdItems.ItemsSource = App.db.Nomenclatures.Where(p => p.ClassId == parent.Id).ToList();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (parent == null)
            {
                MessageBox.Show("Перед редактированием номенклатуры создайет номенклатурные группы!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                NavigationService.Navigate(new ClassesList());
            }
            else
            {
                GrdItems.ItemsSource = App.db.Nomenclatures.Where(p => p.ClassId == parent.Id).ToList();
            }
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NomenclatureForm(0, parent.Id));
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Nomenclature item = GrdItems.SelectedItem as Nomenclature;
                NavigationService.Navigate(new NomenclatureForm(item.Id, item.ClassId));
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Nomenclature item = GrdItems.SelectedItem as Nomenclature;
                if (item.Expenses.Count > 0 || item.Calculations.Count > 0 || item.Structures.Count > 0 || item.Outputs.Count > 0 || item.Plans.Count > 0)
                {
                    MessageBox.Show("Нельзя удалить объект, т.к. на него имеются ссылки!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить: {0}", item.Name), "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        App.db.Nomenclatures.Remove(item);
                        try
                        {
                            App.db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            App.db.UndoChanges();
                            MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                            App.SetLogText("Ошибка удаления номенклатуры\t" + App.user.Name);
                        }
                        GrdItems.ItemsSource = App.db.Nomenclatures.Where(p => p.ClassId == parent.Id).ToList();
                    }
                }
            }
        }
    }
}
