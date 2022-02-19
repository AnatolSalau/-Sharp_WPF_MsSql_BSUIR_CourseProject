using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sebestoimost.Model;

namespace Sebestoimost.Pages
{
    public partial class CostsCalc : Page
    {
        Cost item;
        public CostsCalc(int id)
        {
            InitializeComponent();
            App.db = new dbContext();
            item = App.db.Costs.FirstOrDefault(p => p.Id == id);
            BtnOk.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (item == null)
            {
                NavigationService.Navigate(new CostsList());
            }
            // Удаляем все результаты расчета себестоимости
            foreach (Structure structure in App.db.Structures.Where(p => p.CostId == item.Id))
            {
                App.db.Structures.Remove(structure);
            }
            App.db.SaveChanges();
            // Даты отбора
            DateTime dateStart = new DateTime(item.Date.Year, item.Date.Month, 1, 0, 0, 0);
            DateTime dateEnd = dateStart.AddMonths(1).AddDays(-1);
            dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day, 23, 59, 59);
            // Список документов выпуска
            var outputs = App.db.Outputs.Where(p => p.Date >= dateStart && p.Date <= dateEnd).ToList();
            // Проверка наличия выпуска продукции: если выпуска не было, расчет не проводится
            if (outputs.Count == 0)
            {
                FldStage1.Text = "За месяц не зарегистрирован выпуск продукции!";
                BtnOk.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                FldStage1.Text = "Пройдена";
            }
            // Список номенклатуры выпуска
            List<Nomenclature> oNomenclatures = new List<Nomenclature>();
            foreach (var output in outputs)
            {
                if (oNomenclatures.Count(p => p.Id == output.NomenclatureId) == 0)
                {
                    oNomenclatures.Add(output.Nomenclature);
                }
            }
            // Список номенклатуры с плановыми ценами
            List<NomenclaturePriceData> oNomenclaturePrices = new List<NomenclaturePriceData>();
            foreach (var nomenclature in oNomenclatures)
            {
                var plans = App.db.Plans.Where(p => p.NomenclatureId == nomenclature.Id && p.Date <= dateEnd).OrderByDescending(p => p.Date).ToArray();
                if (plans.Count() > 0)
                {
                    oNomenclaturePrices.Add(new NomenclaturePriceData { Nomenclature = nomenclature, Price = plans[0].Price });
                }
            }
            // Проверка наличия плановых цен: если плановые цены на выпущенную продукцию не зарегистрированы, расчет не проводится
            if (oNomenclatures.Count > oNomenclaturePrices.Count)
            {
                FldStage2.Text = "Не зарегистрированы плановые цены на всю продукцию выпуска!";
                BtnOk.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                FldStage2.Text = "Пройдена";
            }
            // Список документов затрат
            var expenses = App.db.Expenses.Where(p => p.Date >= dateStart && p.Date <= dateEnd).ToList();
            // Проверка наличия регистрации затрат: если затрат не было, дальнейшие расчеты не имеют смысла
            if (expenses.Count == 0)
            {
                FldStage3.Text = "За меясц не зарегистрированы затраты!";
                BtnOk.Visibility = Visibility.Visible;
                return;
            }
            // Список подразделений выпуска
            List<Department> oDepartments = new List<Department>();
            foreach (var output in outputs)
            {
                if (oDepartments.Count(p => p.Id == output.DepartmentId) == 0)
                {
                    oDepartments.Add(output.Department);
                }
            }
            // Список подразделений затрат
            List<Department> eDepartments = new List<Department>();
            foreach (var expense in expenses)
            {
                if (eDepartments.Count(p => p.Id == expense.DepartmentId) == 0)
                {
                    eDepartments.Add(expense.Department);
                }
            }
            // Проверка подразделений
            if (oDepartments.Except(eDepartments).Count() > 0)
            {
                FldStage3.Text = "Подразделения выпуска и затрат не совпадают!";
                BtnOk.Visibility = Visibility.Visible;
                return;
            }
            // Список номенклатурных групп выпуска
            List<Class> oClasses = new List<Class>();
            foreach (var nomenclature in oNomenclatures)
            {
                if (oClasses.Count(p => p.Id == nomenclature.ClassId) == 0)
                {
                    oClasses.Add(nomenclature.Class);
                }
            }
            // Список номенклатурных групп затрат
            List<Class> eClasses = new List<Class>();
            foreach (var expense in expenses)
            {
                if (eClasses.Count(p => p.Id == expense.ClassId) == 0)
                {
                    eClasses.Add(expense.Class);
                }
            }
            // Проверка номенклатурных групп
            if (oClasses.Except(eClasses).Count() > 0)
            {
                FldStage3.Text = "Номенклатурные группы выпуска и затрат не совпадают!";
                BtnOk.Visibility = Visibility.Visible;
                return;
            }
            FldStage3.Text = "Пройдена";
            // Формируем список выпущенной продукции по подразделениям с ценами и количеством
            List<OutputData> oDatas = new List<OutputData>();
            foreach (var department in oDepartments)
            {
                foreach (var nomenclature in oNomenclaturePrices)
                {
                    decimal quantity = outputs.Where(p => p.DepartmentId == department.Id && p.NomenclatureId == nomenclature.NomenclatureId).Sum(p => p.Quantity);
                    if (quantity > 0)
                    {
                        oDatas.Add(new OutputData
                        {
                            Department = department,
                            Nomenclature = nomenclature.Nomenclature,
                            Quantity = quantity,
                            Price = nomenclature.Price,
                            Summa = quantity * nomenclature.Price
                        });
                    }
                }
            }
            // Формируем сводный список затрат
            var gExpenses = expenses.GroupBy(cm => new { cm.Department, cm.Class, cm.Expenditure, cm.Nomenclature },
                (key, group) => new { key.Department, key.Class, key.Expenditure, key.Nomenclature, Summa = group.Sum(p => p.Summa) });
            // Формируем список выпуска по подразделениям и номенклатурным группам
            var gOutputs = oDatas.GroupBy(cm => new { cm.Department, cm.Class },
                (key, group) => new { key.Department, key.Class, Nomenclatures = group.ToList(), Summa = group.Sum(p => p.Summa) });
            // Распределяем затраты на продукцию
            List<CostStructure> costStructures = new List<CostStructure>();
            foreach (var gOutput in gOutputs)
            {
                foreach (var nomenclature in gOutput.Nomenclatures)
                {
                    var costStructure = new CostStructure() { Department = gOutput.Department, Nomenclature = nomenclature.Nomenclature, Quantity = nomenclature.Quantity };
                    foreach (var gExpense in gExpenses.Where(p => p.Department.Id == gOutput.Department.Id && p.Class.Id == gOutput.Class.Id))
                    {
                        var expenseSumma = nomenclature.Summa * gExpense.Summa / gOutput.Summa;
                        costStructure.CostCalculations.Add(new CostCalculation
                        {
                            Expenditure = gExpense.Expenditure,
                            Nomenclature = gExpense.Nomenclature,
                            Summa = expenseSumma
                        });
                    }
                    costStructures.Add(costStructure);
                }
            }
            // Сохраняем затраты в БД
            foreach (var costStructure in costStructures)
            {
                Structure structure = new Structure
                {
                    Cost = item,
                    Nomenclature = costStructure.Nomenclature,
                    Department = costStructure.Department,
                    Quantity = costStructure.Quantity
                };
                App.db.Structures.Add(structure);
                foreach (var costCalculation in costStructure.CostCalculations)
                {
                    Calculation calculation = new Calculation
                    {
                        Structure = structure,
                        Expenditure = costCalculation.Expenditure,
                        Nomenclature = costCalculation.Nomenclature,
                        Summa = costCalculation.Summa
                    };
                    App.db.Calculations.Add(calculation);
                }
            }
            try
            {
                App.db.SaveChanges();
            }
            catch (Exception ex)
            {
                App.db.UndoChanges();
                MessageBox.Show(ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                App.SetLogText("Ошибка сохранения результатов расчета себестоимости\t" + App.user.Name);
                FldStage4.Text = "Не удалось сохранить результаты!";
                BtnOk.Visibility = Visibility.Visible;
                return;
            }
            FldStage4.Text = "Успешно завершен!";
            BtnOk.Visibility = Visibility.Visible;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CostsList());
        }
    }
}
