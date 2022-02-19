using Sebestoimost.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sebestoimost.Pages
{
    public partial class UsersList : Page
    {
        public UsersList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.db = new dbContext();
            GrdItems.ItemsSource = App.db.Users.ToList();
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UsersForm(0));
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                User item = GrdItems.SelectedItem as User;
                NavigationService.Navigate(new UsersForm(item.Id));
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                User item = GrdItems.SelectedItem as User;
                if (item.Id == App.user.Id)
                {
                    MessageBox.Show("Нельзя удалить собственную запись!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (item.Costs.Count > 0)
                {
                    MessageBox.Show("Нельзя удалить объект, т.к. на него имеются ссылки!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить: {0}", item.Name), "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        App.db.Users.Remove(item);
                        try
                        {
                            App.db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            App.db.UndoChanges();
                            MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                            App.SetLogText("Ошибка удаления пользователя\t" + App.user.Name);
                        }
                        GrdItems.ItemsSource = App.db.Users.ToList();
                    }
                }
            }
        }
    }
}
