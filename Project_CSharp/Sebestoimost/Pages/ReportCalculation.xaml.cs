using Sebestoimost.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sebestoimost.Pages
{
    public partial class ReportCalculation : Page
    {
        string title;
        public ReportCalculation()
        {
            InitializeComponent();
            App.db = new dbContext();
            title = "Калькуляция";
            FldDateAt.SelectedDate = new DateTime(2020, 1, 1);
            FldDateTo.SelectedDate = new DateTime(2020, 1, 31);
            ReportCreate();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var doc = fldBrowser.Document as mshtml.IHTMLDocument2;
                doc.execCommand("Print", true, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка печати", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            ReportCreate();
        }

        private void ReportCreate()
        {
            DateTime dateAt = (DateTime)FldDateAt.SelectedDate;
            DateTime dateTo = (DateTime)FldDateTo.SelectedDate;
            dateAt = new DateTime(dateAt.Year, dateAt.Month, 1, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, 1, 0, 0, 0);
            dateTo = dateTo.AddMonths(1).AddDays(-1);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
            FldDateAt.SelectedDate = dateAt;
            FldDateTo.SelectedDate = dateTo;
            var costs = App.db.Costs.Where(p => p.Date >= dateAt && p.Date <= dateTo).ToList();
            List<Calculation> calculations = new List<Calculation>();
            foreach (var cost in costs)
            {
                foreach (var structure in App.db.Structures.Where(p => p.CostId == cost.Id))
                {
                    foreach (var calculation in App.db.Calculations.Where(p => p.StructureId == structure.Id))
                    {
                        calculations.Add(calculation);
                    }
                }
            }
            var gReport = calculations.GroupBy(
                cm => new { cm.Structure },
                (key, group) => new {
                    key.Structure,
                    Nomenclatures = group.ToList().GroupBy(
                    cm1 => new { cm1.Expenditure, cm1.Nomenclature },
                    (key1, group1) => new { key1.Expenditure, key1.Nomenclature, Summa = group1.Sum(p => p.Summa) }
                ),
                    Summa = group.Sum(p => p.Summa)
                });

            string reportStr = ReportHtmlHelper.PageStart(title) + ReportHtmlHelper.ReportHeader(title, dateAt, dateTo);
            string thead = ReportHtmlHelper.Tr(new string[] { "Продукция / Затрата", "Количество", "На ед. выпуска", "Сумма" });
            string tbody = "";
            foreach (var structure in gReport)
            {
                tbody += ReportHtmlHelper.TrSub(new string[]
                    {
                        structure.Structure.Department.Name + " / " + structure.Structure.Nomenclature.Name,
                        string.Format("{0:0.000} {1}", structure.Structure.Quantity, structure.Structure.Nomenclature.Class.Measure.Name),
                        string.Format("{0:0.00}", structure.Summa / structure.Structure.Quantity),
                        string.Format("{0:0.00}", structure.Summa)
                    });
                foreach (var expenditure in structure.Nomenclatures)
                {
                    tbody += ReportHtmlHelper.Tr(new string[]
                        {
                            expenditure.Nomenclature.Name + " (" + expenditure.Expenditure.Name + ")",
                            "",
                            string.Format("{0:0.00}", expenditure.Summa / structure.Structure.Quantity),
                            string.Format("{0:0.00}", expenditure.Summa)
                        });
                }
            }
            reportStr += ReportHtmlHelper.Table(thead, tbody);
            reportStr += ReportHtmlHelper.PageEnd();
            fldBrowser.NavigateToString(reportStr);
        }

        private void FldDateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FldDateTo.SelectedDate < FldDateAt.SelectedDate)
            {
                FldDateAt.SelectedDate = FldDateTo.SelectedDate;
            }
        }

        private void FldDateAt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FldDateTo.SelectedDate < FldDateAt.SelectedDate)
            {
                FldDateTo.SelectedDate = FldDateAt.SelectedDate;
            }
        }
    }
}
