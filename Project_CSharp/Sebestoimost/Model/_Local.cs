using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebestoimost.Model
{
    public class NomenclaturePriceData
    {
        public int NomenclatureId { get { return Nomenclature.Id; } }
        public Nomenclature Nomenclature { get; set; }
        public decimal Price { get; set; }
        public int ClassId { get { return Nomenclature.ClassId; } }
        public Class Class { get { return Nomenclature.Class; } }
    }
    public class OutputData
    {
        public int DepartmentId { get { return Department.Id; } }
        public Department Department { get; set; }
        public int ClassId { get { return Nomenclature.ClassId; } }
        public Class Class { get { return Nomenclature.Class; } }
        public int NomenclatureId { get { return Nomenclature.Id; } }
        public Nomenclature Nomenclature { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Summa { get; set; }
    }
    public class CostStructure
    {
        public Department Department { get; set; }
        public Nomenclature Nomenclature { get; set; }
        public decimal Quantity { get; set; }
        public List<CostCalculation> CostCalculations { get; set; }
        public CostStructure()
        {
            CostCalculations = new List<CostCalculation>();
        }

    }
    public class CostCalculation
    {
        public Expenditure Expenditure { get; set; }
        public Nomenclature Nomenclature { get; set; }
        public decimal Summa { get; set; }
    }
    public class ALog
    {
        public DateTime Date { get; set; }
        public string Event { get; set; }
        public string User { get; set; }

        public ALog(string line)
        {
            string[] parts = line.Split(new char[] { '\t' });
            if (parts.Length > 0)
            {
                Date = DateTime.Parse(parts[0]);
                if (parts.Length > 1)
                {
                    Event = parts[1];
                    if (parts.Length > 2)
                    {
                        User = parts[2];
                    }
                }
            }
        }
    }
}
