using Sebestoimost.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Sebestoimost.Pages
{
    public partial class ExpendituresList : Page
    {
        public ExpendituresList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.db = new dbContext();
            GrdItems.ItemsSource = App.db.Expenditures.ToList();
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExpendituresForm(0));
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Expenditure item = GrdItems.SelectedItem as Expenditure;
                NavigationService.Navigate(new ExpendituresForm(item.Id));
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Expenditure item = GrdItems.SelectedItem as Expenditure;
                if (item.Expenses.Count > 0 || item.Calculations.Count > 0)
                {
                    MessageBox.Show("Нельзя удалить объект, т.к. на него имеются ссылки!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить: {0}", item.Name), "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        App.db.Expenditures.Remove(item);
                        try
                        {
                            App.db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            App.db.UndoChanges();
                            MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                            App.SetLogText("Ошибка удаления статьи затрат\t" + App.user.Name);
                        }
                        GrdItems.ItemsSource = App.db.Expenditures.ToList();
                    }
                }
            }
        }

    }
}
