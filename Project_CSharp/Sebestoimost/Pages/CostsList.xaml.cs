using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sebestoimost.Model;

namespace Sebestoimost.Pages
{
    public partial class CostsList : Page
    {
        public CostsList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.db = new dbContext();
            // Предустановки фильтра
            FltrGrid.Visibility = Visibility.Collapsed;
            FltrDateCheck.IsChecked = false;
            FltrDateAt.SelectedDate = DateTime.Now;
            FltrDateTo.SelectedDate = DateTime.Now;
            //
            FillData();
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CostsForm(0));
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Cost item = GrdItems.SelectedItem as Cost;
                NavigationService.Navigate(new CostsForm(item.Id));
            }
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (GrdItems.SelectedItem != null)
            {
                Cost item = GrdItems.SelectedItem as Cost;
                if (MessageBox.Show(string.Format("Вы действительно хотите удалить документ №{0} от {1:dd.MM.yyyy}", item.Id, item.Date), "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    App.db.Costs.Remove(item);
                    try
                    {
                        App.db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        App.db.UndoChanges();
                        MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Warning);
                        App.SetLogText("Ошибка удаления расчета себестоимости\t" + App.user.Name);
                    }
                    FillData();
                }
            }
        }

        private void MenuFilter_Click(object sender, RoutedEventArgs e)
        {
            if (FltrGrid.Visibility == Visibility.Visible)
            {
                FltrGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                FltrGrid.Visibility = Visibility.Visible;
            }
        }

        private void FltrBtn_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        private void FltrCancel_Click(object sender, RoutedEventArgs e)
        {
            FltrDateCheck.IsChecked = false;
            FillData();
        }

        private void FillData()
        {
            List<Cost> fltrList = App.db.Costs.ToList();
            if (FltrDateCheck.IsChecked == true && FltrDateAt.SelectedDate <= FltrDateTo.SelectedDate)
            {
                fltrList = fltrList.Where(p => p.Date >= FltrDateAt.SelectedDate && p.Date <= FltrDateTo.SelectedDate).ToList();
            }
            else
            {
                FltrDateCheck.IsChecked = false;
            }
            GrdItems.ItemsSource = fltrList;
        }
    }
}
