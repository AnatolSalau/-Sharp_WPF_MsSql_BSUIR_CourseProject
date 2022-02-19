using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Sebestoimost.Model
{
    public class dbContext : DbContext
    {
        static dbContext()
        {
            Database.SetInitializer(new DatabaseInitializer());
        }
        public dbContext() : base("name=dbContext") { }

        public virtual DbSet<Calculation> Calculations { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Expenditure> Expenditures { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }
        public virtual DbSet<Nomenclature> Nomenclatures { get; set; }
        public virtual DbSet<NomenclatureType> NomenclatureTypes { get; set; }
        public virtual DbSet<Output> Outputs { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Structure> Structures { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Costs)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Expenditure>()
                .HasMany(e => e.Calculations)
                .WithRequired(e => e.Expenditure)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Nomenclature>()
                .HasMany(e => e.Calculations)
                .WithRequired(e => e.Nomenclature)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Structure>()
                .HasMany(e => e.Calculations)
                .WithRequired(e => e.Structure)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Cost>()
                .HasMany(e => e.Structures)
                .WithRequired(e => e.Cost)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Expenditure>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.Expenditure)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Nomenclature>()
                .HasMany(e => e.Structures)
                .WithRequired(e => e.Nomenclature)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Structures)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Measure>()
                .HasMany(e => e.Classes)
                .WithRequired(e => e.Measure)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Outputs)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Class>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Nomenclature>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.Nomenclature)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Class>()
                .HasMany(e => e.Nomenclatures)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Nomenclature>()
                .HasMany(e => e.Outputs)
                .WithRequired(e => e.Nomenclature)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<NomenclatureType>()
                .HasMany(e => e.Nomenclatures)
                .WithRequired(e => e.NomenclatureType)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Nomenclature>()
                .HasMany(e => e.Plans)
                .WithRequired(e => e.Nomenclature)
                .WillCascadeOnDelete(false);
            //
            modelBuilder.Entity<Structure>()
                .Property(e => e.Quantity)
                .HasPrecision(15, 3);
            modelBuilder.Entity<Output>()
                .Property(e => e.Quantity)
                .HasPrecision(15, 3);
            modelBuilder.Entity<Calculation>()
                .Property(e => e.Summa)
                .HasPrecision(15, 2);
            modelBuilder.Entity<Plan>()
                .Property(e => e.Price)
                .HasPrecision(15, 2);
            modelBuilder.Entity<Expense>()
                .Property(e => e.Summa)
                .HasPrecision(15, 2);
            //
            base.OnModelCreating(modelBuilder);
        }

        public void UndoChanges()
        {
            foreach (DbEntityEntry entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            }
        }
    }

    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<dbContext>
    {
        protected override void Seed(dbContext context)
        {
            // Роли
            Role roleAdmin = new Role() { Id = 1, Name = "Администратор" };
            Role roleEconom = new Role() { Id = 2, Name = "Экономист" };
            Role roleBuh = new Role() { Id = 3, Name = "Бухгалтер" };
            Role roleDisp = new Role() { Id = 4, Name = "Диспетчер производства" };
            context.Roles.Add(roleAdmin);
            context.Roles.Add(roleEconom);
            context.Roles.Add(roleBuh);
            context.Roles.Add(roleDisp);
            // Пользователи
            string pswrd = App.GetMD5("1");
            User userAdmin = new User() { Name = "Иванов И.И.", Password = pswrd, Role = roleAdmin, Enabled = true };
            User userEconom = new User() { Name = "Петрова П.П.", Password = pswrd, Role = roleEconom, Enabled = true };
            User userBuh = new User() { Name = "Сидорова С.С.", Password = pswrd, Role = roleBuh, Enabled = true };
            User userDisp = new User() { Name = "Соколов С.С.", Password = pswrd, Role = roleDisp, Enabled = true };
            context.Users.Add(userAdmin);
            context.Users.Add(userEconom);
            context.Users.Add(userBuh);
            context.Users.Add(userDisp);
            // Типы номенклатуры
            NomenclatureType typeProduct = new NomenclatureType() { Id = 1, Name = "Продукция" };
            NomenclatureType typeExpense = new NomenclatureType() { Id = 2, Name = "Затрата" };
            context.NomenclatureTypes.Add(typeProduct);
            context.NomenclatureTypes.Add(typeExpense);
            // Единицы измерения
            Measure measureSht = new Measure() { Name = "шт" };
            Measure measureRub = new Measure() { Name = "руб" };
            Measure measureL = new Measure() { Name = "л" };
            Measure measureT = new Measure() { Name = "т" };
            Measure measureCub = new Measure() { Name = "м3" };
            context.Measures.Add(measureSht);
            context.Measures.Add(measureRub);
            context.Measures.Add(measureL);
            context.Measures.Add(measureT);
            // Номенклатурные группы
            Class classProduct1 = new Class() { Name = "Лопаты", Measure = measureSht };
            Class classProduct2 = new Class() { Name = "Инструменты", Measure = measureSht };
            Class classWood = new Class() { Name = "Дерево, пиломатериалы", Measure = measureCub };
            Class classMetal = new Class() { Name = "Металл весовой", Measure = measureT };
            Class classMat = new Class() { Name = "Материалы штучные", Measure = measureSht };
            Class classOther = new Class() { Name = "Прочие затраты", Measure = measureRub };
            context.Classes.Add(classProduct1);
            context.Classes.Add(classProduct2);
            context.Classes.Add(classWood);
            context.Classes.Add(classMetal);
            context.Classes.Add(classOther);
            // Номенклатура
            Nomenclature nomShtyk = new Nomenclature() { Name = "Лопата штыковая", NomenclatureType = typeProduct, Class = classProduct1, Description = "Простая штыковая лопата" };
            Nomenclature nomSovok = new Nomenclature() { Name = "Лопата совковая", NomenclatureType = typeProduct, Class = classProduct1, Description = "Обычная совковая лопата" };
            Nomenclature nomMolot = new Nomenclature() { Name = "Молоток", NomenclatureType = typeProduct, Class = classProduct2, Description = "Самый обычный молоток" };
            Nomenclature nomTopor = new Nomenclature() { Name = "Топор", NomenclatureType = typeProduct, Class = classProduct2, Description = "Самый обычный топор" };
            Nomenclature nomBrus = new Nomenclature() { Name = "Брус деревянный", NomenclatureType = typeExpense, Class = classWood, Description = "Брус, береза, 50х50" };
            Nomenclature nomDoska = new Nomenclature() { Name = "Доска деревянная", NomenclatureType = typeExpense, Class = classWood, Description = "Доска, береза, 50" };
            Nomenclature nomStal = new Nomenclature() { Name = "Прокат листовой", NomenclatureType = typeExpense, Class = classMetal, Description = "Сталь 30ХГС" };
            Nomenclature nomRMolot = new Nomenclature() { Name = "Рабочая часть молотка", NomenclatureType = typeExpense, Class = classMat, Description = "Литой" };
            Nomenclature nomRTopor = new Nomenclature() { Name = "Рабочая часть топора", NomenclatureType = typeExpense, Class = classMat, Description = "Кованый, сталь 60Г" };
            Nomenclature nomElektr = new Nomenclature() { Name = "Электроэнергия", NomenclatureType = typeExpense, Class = classOther, Description = "" };
            Nomenclature nomZp = new Nomenclature() { Name = "Заработная плата", NomenclatureType = typeExpense, Class = classOther, Description = "" };
            Nomenclature nomSoc = new Nomenclature() { Name = "Соцнужды", NomenclatureType = typeExpense, Class = classOther, Description = "" };
            Nomenclature nomAmort = new Nomenclature() { Name = "Амортизация", NomenclatureType = typeExpense, Class = classOther, Description = "" };
            Nomenclature nomPr = new Nomenclature() { Name = "Прочие затраты", NomenclatureType = typeExpense, Class = classOther, Description = "" };
            context.Nomenclatures.Add(nomMolot);
            context.Nomenclatures.Add(nomTopor);
            context.Nomenclatures.Add(nomShtyk);
            context.Nomenclatures.Add(nomSovok);
            context.Nomenclatures.Add(nomBrus);
            context.Nomenclatures.Add(nomDoska);
            context.Nomenclatures.Add(nomStal);
            context.Nomenclatures.Add(nomRMolot);
            context.Nomenclatures.Add(nomRTopor);
            context.Nomenclatures.Add(nomElektr);
            context.Nomenclatures.Add(nomZp);
            context.Nomenclatures.Add(nomSoc);
            context.Nomenclatures.Add(nomAmort);
            context.Nomenclatures.Add(nomPr);
            // Подразделения
            Department dep1 = new Department() { Name = "Цех №1" };
            Department dep2 = new Department() { Name = "Цех №2" };
            context.Departments.Add(dep1);
            context.Departments.Add(dep2);
            // Статьи затрат
            Expenditure expenditureMat = new Expenditure() { Name = "Материальные затраты" };
            Expenditure expenditureEnerg = new Expenditure() { Name = "Энергоресурсы" };
            Expenditure expenditureZp = new Expenditure() { Name = "Заработная плата" };
            Expenditure expenditureOther = new Expenditure() { Name = "Прочие затраты" };
            context.Expenditures.Add(expenditureMat);
            context.Expenditures.Add(expenditureEnerg);
            context.Expenditures.Add(expenditureZp);
            context.Expenditures.Add(expenditureOther);
            // Даты для ввода
            DateTime date1 = new DateTime(2020, 11, 1);
            DateTime date10 = new DateTime(2020, 11, 10);
            DateTime date17 = new DateTime(2020, 11, 17);
            DateTime date24 = new DateTime(2020, 11, 24);
            DateTime date30 = new DateTime(2020, 11, 30);
            // Плановые цены
            context.Plans.Add(new Plan() { Date = date1, Nomenclature = nomShtyk, Price = 6.2M });
            context.Plans.Add(new Plan() { Date = date1, Nomenclature = nomSovok, Price = 7.1M });
            context.Plans.Add(new Plan() { Date = date1, Nomenclature = nomMolot, Price = 12.75M });
            context.Plans.Add(new Plan() { Date = date1, Nomenclature = nomTopor, Price = 15.4M });
            // Выпуск
            context.Outputs.Add(new Output() { Date = date10, Nomenclature = nomShtyk, Department = dep1, Quantity = 400M });
            context.Outputs.Add(new Output() { Date = date17, Nomenclature = nomShtyk, Department = dep1, Quantity = 500M });
            context.Outputs.Add(new Output() { Date = date24, Nomenclature = nomShtyk, Department = dep1, Quantity = 520M });
            context.Outputs.Add(new Output() { Date = date30, Nomenclature = nomShtyk, Department = dep1, Quantity = 480M });
            context.Outputs.Add(new Output() { Date = date10, Nomenclature = nomSovok, Department = dep1, Quantity = 800M });
            context.Outputs.Add(new Output() { Date = date17, Nomenclature = nomSovok, Department = dep1, Quantity = 900M });
            context.Outputs.Add(new Output() { Date = date24, Nomenclature = nomSovok, Department = dep1, Quantity = 1000M });
            context.Outputs.Add(new Output() { Date = date30, Nomenclature = nomSovok, Department = dep1, Quantity = 1200M });
            context.Outputs.Add(new Output() { Date = date10, Nomenclature = nomMolot, Department = dep2, Quantity = 500M });
            context.Outputs.Add(new Output() { Date = date17, Nomenclature = nomMolot, Department = dep2, Quantity = 600M });
            context.Outputs.Add(new Output() { Date = date24, Nomenclature = nomMolot, Department = dep2, Quantity = 610M });
            context.Outputs.Add(new Output() { Date = date30, Nomenclature = nomMolot, Department = dep2, Quantity = 570M });
            context.Outputs.Add(new Output() { Date = date10, Nomenclature = nomTopor, Department = dep2, Quantity = 750M });
            context.Outputs.Add(new Output() { Date = date17, Nomenclature = nomTopor, Department = dep2, Quantity = 880M });
            context.Outputs.Add(new Output() { Date = date24, Nomenclature = nomTopor, Department = dep2, Quantity = 920M });
            context.Outputs.Add(new Output() { Date = date30, Nomenclature = nomTopor, Department = dep2, Quantity = 820M });
            context.Outputs.Add(new Output() { Date = date10, Nomenclature = nomSovok, Department = dep2, Quantity = 200M });
            context.Outputs.Add(new Output() { Date = date17, Nomenclature = nomSovok, Department = dep2, Quantity = 250M });
            context.Outputs.Add(new Output() { Date = date24, Nomenclature = nomSovok, Department = dep2, Quantity = 400M });
            context.Outputs.Add(new Output() { Date = date30, Nomenclature = nomSovok, Department = dep2, Quantity = 420M });
            // Затраты
            // Цех 1 - Лопаты
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomBrus, Summa = 3625M, Expenditure = expenditureMat, Department = dep1, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomStal, Summa = 2649.6M, Expenditure = expenditureMat, Department = dep1, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomElektr, Summa = 3712M, Expenditure = expenditureEnerg, Department = dep1, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomZp, Summa = 9870M, Expenditure = expenditureZp, Department = dep1, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomSoc, Summa = 6704.25M, Expenditure = expenditureZp, Department = dep1, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomAmort, Summa = 2175M, Expenditure = expenditureOther, Department = dep1, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomPr, Summa = 10857M, Expenditure = expenditureOther, Department = dep1, Class = classProduct1 });
            // Цех 2 - Инструменты
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomDoska, Summa = 1688.05M, Expenditure = expenditureMat, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomRMolot, Summa = 7410M, Expenditure = expenditureMat, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomRTopor, Summa = 12132M, Expenditure = expenditureMat, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomElektr, Summa = 4408.96M, Expenditure = expenditureEnerg, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomZp, Summa = 18288M, Expenditure = expenditureZp, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomSoc, Summa = 8419.95M, Expenditure = expenditureZp, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomAmort, Summa = 8475M, Expenditure = expenditureOther, Department = dep2, Class = classProduct2 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomPr, Summa = 20116.8M, Expenditure = expenditureOther, Department = dep2, Class = classProduct2 });
            // Цех 2 - Лопаты
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomBrus, Summa = 793.75M, Expenditure = expenditureMat, Department = dep2, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomStal, Summa = 640.08M, Expenditure = expenditureMat, Department = dep2, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomElektr, Summa = 731.52M, Expenditure = expenditureEnerg, Department = dep2, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomZp, Summa = 1905M, Expenditure = expenditureZp, Department = dep2, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomSoc, Summa = 1466.85M, Expenditure = expenditureZp, Department = dep2, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomAmort, Summa = 1333.5M, Expenditure = expenditureOther, Department = dep2, Class = classProduct1 });
            context.Expenses.Add(new Expense() { Date = date30, Nomenclature = nomPr, Summa = 2095.5M, Expenditure = expenditureOther, Department = dep2, Class = classProduct1 });
            // Расчеты
            context.Costs.Add(new Cost() { Date = date30, User = userEconom });
            //
            context.SaveChanges();
            //
            base.Seed(context);
        }
    }
}
