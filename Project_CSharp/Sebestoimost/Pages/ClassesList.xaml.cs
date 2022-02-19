using Sebestoimost.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class ClassesList : Page
    {
        public ClassesList()
        {
            InitializeComponent();
        }

        private void MenuNomenclature_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Class item = GrdItems.SelectedItem as Class;
                NavigationService.Navigate(new NomenclatureList(item.Id));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.db = new dbContext();
            GrdItems.ItemsSource = App.db.Classes.ToList();
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClassesForm(0));
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Class item = GrdItems.SelectedItem as Class;
                NavigationService.Navigate(new ClassesForm(item.Id));
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Class item = GrdItems.SelectedItem as Class;
                if (item.Nomenclatures.Count > 0 || item.Expenses.Count > 0)
                {
                    MessageBox.Show("Нельзя удалить объект, т.к. на него имеются ссылки!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить: {0}", item.Name), "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        App.db.Classes.Remove(item);
                        try
                        {
                            App.db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            App.db.UndoChanges();
                            MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                            App.SetLogText("Ошибка удаления номенклатурной группы\t" + App.user.Name);
                        }
                        GrdItems.ItemsSource = App.db.Classes.ToList();
                    }
                }
            }
        }

    }
}
