using System.Windows;

namespace Sebestoimost
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            LblUser.Text = App.user.Name;
            if (!(App.user.RoleId == 1))
                MenuOption.Visibility = Visibility.Collapsed;
            FrameHelp.Visibility = Visibility.Collapsed;
            FrameHelp.Navigate(new Helps.HelpContent());
            FrameMain.Navigate(new Pages.NomenclatureList(0));
            LblStatus.Text = "Номенклатура";
        }

        private void MenuNomenclature_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.NomenclatureList(0));
            LblStatus.Text = "Номенклатура";
        }

        private void MenuClasses_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ClassesList());
            LblStatus.Text = "Номенклатурные группы";
        }

        private void MenuDepartments_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.DepartmentsList());
            LblStatus.Text = "Подразделения";
        }

        private void MenuExpenditures_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ExpendituresList());
            LblStatus.Text = "Статьи затрат";
        }

        private void MenuMeasures_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.MeasuresList());
            LblStatus.Text = "Единицы измерения";
        }

        private void MenuUsers_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.UsersList());
            LblStatus.Text = "Пользователи";
        }

        private void MenuExpenses_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ExpensesList());
            LblStatus.Text = "Затраты";
        }

        private void MenuOutputs_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.OutputsList());
            LblStatus.Text = "Выпуск";
        }

        private void MenuPlans_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.PlansList());
            LblStatus.Text = "Плановые цены";
        }

        private void MenuCosts_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.CostsList());
            LblStatus.Text = "Расчеты себестоимости";
        }

        private void MenuReportStructure_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ReportStructure());
            LblStatus.Text = "Структура себестоимости";
        }

        private void MenuReportCalculation_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ReportCalculation());
            LblStatus.Text = "Калькуляция";
        }

        private void MenuReportCost_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ReportCost());
            LblStatus.Text = "Распределение затрат";
        }

        private void MenuReportCostPrice_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ReportCostPrice());
            LblStatus.Text = "Себестоимость";
        }

        private void FrameMain_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FrameMain.NavigationService.RemoveBackEntry();
        }

        private void MenuLog_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.LogFile());
            LblStatus.Text = "Лог-файл";
        }

        private void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            if (FrameHelp.Visibility == Visibility.Visible)
            {
                FrameHelp.Visibility = Visibility.Collapsed;
            }
            else
            {
                FrameHelp.Visibility = Visibility.Visible;
            }
        }
    }
}
