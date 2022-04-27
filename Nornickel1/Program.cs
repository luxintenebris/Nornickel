// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
using AdoEfDemo;
using Nornickel1.EF;

static void AdoNetDemo()
{
    var connection = new SqlConnection(@"Server=.\SQLEXPRESS01;Database=hw_1;Trusted_Connection=True;");
    connection.Open();
    Console.WriteLine("Сортировка по убыванию числа побед: ");

    var command = connection.CreateCommand();
    command.CommandText = "SELECT Athlete.FIO, Category.Title FROM Athlete INNER JOIN Category ON Athlete.Weight_Category = Category.ID ORDER BY Wins DESC";
    var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"FIO = {reader.GetString(reader.GetOrdinal("FIO"))}, Category = {reader.GetString(reader.GetOrdinal("Title"))} ");
    } 
    reader.Close();

    Console.WriteLine("Сортировка по возрастанию числа побед: ");
    command = connection.CreateCommand();
    command.CommandText = "SELECT Athlete.FIO, Category.Title FROM Athlete INNER JOIN Category ON Athlete.Weight_Category = Category.ID ORDER BY Wins ASC";

    reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"FIO = {reader.GetString(reader.GetOrdinal("FIO"))}, Category = {reader.GetString(reader.GetOrdinal("Title"))} ");
    }
    reader.Close();

    Console.WriteLine("Первые 3 молодых бойца");
   
    command = connection.CreateCommand();
    command.CommandText = "SELECT TOP(3) FIO, Weight_Category FROM Athlete WHERE (Wins + Lose > 10 AND Year(GetDate()) - YearStart < 5) ORDER BY (Wins/Lose) DESC";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"FIO = {reader.GetString(reader.GetOrdinal("FIO"))}");
    }
    reader.Close();

    Console.WriteLine("Перспективные, > 10 матчей, коэф. побед выше среднего");

    command = connection.CreateCommand();
    command.CommandText = "WITH AVER AS(SELECT AVG(Wins / Lose) AS AV FROM Athlete) SELECT TOP(3) FIO, Weight_Category FROM Athlete, AVER WHERE (Wins + Lose > 10 AND (Wins/Lose > AVER.AV)) ORDER BY(Wins/Lose) DESC ";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"FIO = {reader.GetString(reader.GetOrdinal("FIO"))}");
    }
    reader.Close();

    Console.WriteLine("Весовая категория и средняя продолжительность карьеры");
    command = connection.CreateCommand();
    command.CommandText = "SELECT Category.Title, AVG(YEAR(GETDATE()) - Athlete.YearStart) AS Career FROM Athlete INNER JOIN Category ON Athlete.Weight_Category = Category.ID GROUP BY Category.Title";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"{reader.GetString(reader.GetOrdinal("Title"))} : {reader.GetInt32(1)}");
    }
    reader.Close();

    Console.WriteLine("Весовая категория и средняя продолжительность карьеры");
    command = connection.CreateCommand();
    command.CommandText = "DECLARE @maxCar INT SELECT @maxCar = MAX (YEAR(GETDATE())-YearStart) from Athlete SELECT ID from Athlete WHERE YEAR(GETDATE())-YearStart = @maxCar";
    reader = command.ExecuteReader();
    int id1 = 0 ;
    while (reader.Read())
    {
         id1 = reader.GetInt32(0);
    }
   
    reader.Close();
    command = connection.CreateCommand();
    command.CommandText = $"DELETE Athlete where Athlete.ID = {id1}";
    reader = command.ExecuteReader();
    reader.Close();

    connection.Close();
}

AdoNetDemo();

//LinqDemo();


static void LinqDemo()
{
    var db = new Database();

    Console.WriteLine("Сортировка по убыванию числа побед: ");
    foreach (var g in db.Athletes.OrderByDescending(e => e.wins))
    {
        Console.WriteLine($"{g.FIO}: {g.Category.Title}");
    }
    Console.WriteLine();
    Console.WriteLine("Сортировка по возрастанию числа побед: ");
    foreach (var g in db.Athletes.OrderBy(e => e.wins))
    {
        Console.WriteLine($"{g.FIO}: {g.Category.Title}");
    }
    Console.WriteLine();
    Console.WriteLine("Первые 3 молодых бойца");
    var year = DateTime.Now.Year;
    var c = 0;
    foreach (var g in db.Athletes.OrderByDescending(e => e.wins / e.lose))
    {
        if (year - g.year < 5 & (g.wins + g.lose) > 10 & c < 3)
        {
            Console.WriteLine($"{g.FIO}: {g.Category.Title}");
            c++;
        }
    }
    Console.WriteLine();
    Console.WriteLine("Перспективные, > 10 матчей, коэф. побед выше среднего");
    var avg = db.Athletes.Average(e => e.wins / e.lose);
    Console.WriteLine(avg);
    foreach (var g in db.Athletes)
    {
        if ((g.wins + g.lose) > 10 & (g.wins / g.lose) > avg)
        {
            Console.WriteLine($"{g.FIO}: {g.Category.Title}");
        }
    }
    Console.WriteLine();
    Console.WriteLine("Весовая категория и средняя продолжительность карьеры");
    foreach (var g in db.Athletes.GroupBy(e => e.Category))
    {
        Console.WriteLine($"{g.Key.Title}: {g.Average(e => year - e.year)}");
    }
    Console.WriteLine();
    Console.WriteLine("Рандомный бой");

    Random rand = new Random();
    int toSkip1 = rand.Next(0, db.Athletes.Count);
    var frst = db.Athletes.Skip(toSkip1).Take(1).First();
    int toSkip2 = rand.Next(0, db.Athletes.Count);
    var scnd = db.Athletes.Skip(toSkip2).Take(1).First();
    while (toSkip1 == toSkip2 & frst.Category != scnd.Category)
    {
        toSkip2 = rand.Next(0, db.Athletes.Count);
        scnd = db.Athletes.Skip(toSkip2).Take(1).First();
    }
    Console.WriteLine($"1: {frst.FIO}: {frst.wins}  {frst.lose}");
    Console.WriteLine($"2: {scnd.FIO}: {scnd.wins}  {scnd.lose}");
    int value = rand.Next();
    if (value % 2 == 0)
    {
        scnd.lose++;
        frst.wins++;
    }
    else
    {
        frst.lose++;
        scnd.wins++;
    }
    Console.WriteLine("После боя:");
    Console.WriteLine($"1: {frst.FIO}: {frst.wins}  {frst.lose}");
    Console.WriteLine($"2: {scnd.FIO}: {scnd.wins}  {scnd.lose}");
    Console.WriteLine();
    Console.WriteLine("Самый продолжительный по карьере боксёр");
    Console.WriteLine("До чистки: ");
    foreach (var g in db.Athletes)
    {
        Console.WriteLine($"{g.FIO}");
    }
    Console.WriteLine();
    var max = 0;
    var dif = 0;
    foreach (var g in db.Athletes)
    {
        dif = year - g.year;
        if (dif > max) max = dif;
    }
    foreach (var g in db.Athletes)
    {
        dif = year - g.year;
        if (dif == max)
        {
            db.Athletes.Remove(g);
            break;
        }
    }
    Console.WriteLine("После чистки: ");
    foreach (var g in db.Athletes)
    {
        Console.WriteLine($"{g.FIO}");
    }
}
//LinqDemo();

static void EF()
{
    DbAthContext db = new DbAthContext();
    db.CreateIfNotEx();
    Console.WriteLine("Сортировка по убыванию числа побед: ");
    foreach (var g in db.Athletes.OrderByDescending(e => e.wins))
    {
        Console.WriteLine($"{g.FIO}: {g.Category.Title}");
    }
    Console.WriteLine();
    Console.WriteLine("Сортировка по возрастанию числа побед: ");
    foreach (var g in db.Athletes.OrderBy(e => e.wins))
    {
        Console.WriteLine($"{g.FIO}: {g.Category.Title}");
    }
    Console.WriteLine();
    Console.WriteLine("Первые 3 молодых бойца");
    var year = DateTime.Now.Year;
    var c = 0;
    foreach (var g in db.Athletes.OrderByDescending(e => e.wins / e.lose))
    {
        if (year - g.year < 5 & (g.wins + g.lose) > 10 & c < 3)
        {
            Console.WriteLine($"{g.FIO}: {g.Category.Title}");
            c++;
        }
    }
    Console.WriteLine();
    Console.WriteLine("Перспективные, > 10 матчей, коэф. побед выше среднего");
    var avg = db.Athletes.Average(e => e.wins / e.lose);
    Console.WriteLine(avg);
    foreach (var g in db.Athletes)
    {
        if ((g.wins + g.lose) > 10 & (g.wins / g.lose) > avg)
        {
            Console.WriteLine($"{g.FIO}: {g.Category.Title}");
        }
    }
    Console.WriteLine();
    Console.WriteLine("Весовая категория и средняя продолжительность карьеры");
    foreach (var g in db.Athletes.GroupBy(e => e.Category))
    {
        Console.WriteLine($"{g.Key.Title}: {g.Average(e => year - e.year)}");
    }
    Console.WriteLine();
    Console.WriteLine("Рандомный бой");

    Random rand = new Random();
    int toSkip1 = rand.Next(0, db.Athletes.Count());
    var frst = db.Athletes.Skip(toSkip1).Take(1).First();
    int toSkip2 = rand.Next(0, db.Athletes.Count());
    var scnd = db.Athletes.Skip(toSkip2).Take(1).First();
    while (toSkip1 == toSkip2 & frst.Category != scnd.Category)
    {
        toSkip2 = rand.Next(0, db.Athletes.Count());
        scnd = db.Athletes.Skip(toSkip2).Take(1).First();
    }
    Console.WriteLine($"1: {frst.FIO}: {frst.wins}  {frst.lose}");
    Console.WriteLine($"2: {scnd.FIO}: {scnd.wins}  {scnd.lose}");
    int value = rand.Next();
    if (value % 2 == 0)
    {
        scnd.lose++;
        frst.wins++;
    }
    else
    {
        frst.lose++;
        scnd.wins++;
    }
    Console.WriteLine("После боя:");
    Console.WriteLine($"1: {frst.FIO}: {frst.wins}  {frst.lose}");
    Console.WriteLine($"2: {scnd.FIO}: {scnd.wins}  {scnd.lose}");
    Console.WriteLine();
    Console.WriteLine("Самый продолжительный по карьере боксёр");
    Console.WriteLine("До чистки: ");
    foreach (var g in db.Athletes)
    {
        Console.WriteLine($"{g.FIO}");
    }
    Console.WriteLine();
    var max = 0;
    var dif = 0;
    foreach (var g in db.Athletes)
    {
        dif = year - g.year;
        if (dif > max) max = dif;
    }
    foreach (var g in db.Athletes)
    {
        dif = year - g.year;
        if (dif == max)
        {
            db.Athletes.Remove(g);
            break;
        }
    }
    Console.WriteLine("После чистки: ");
    foreach (var g in db.Athletes)
    {
        Console.WriteLine($"{g.FIO}");
    }
}



/*static void EF1()
{
    using (DbAthContext db = new DbAthContext())
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Console.OutputEncoding = Encoding.GetEncoding(1251);

        // пересоздаем базу данных
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.SaveChanges();

        Category1 high = new Category1 { Title = "Тяжелый вес" };
        Category1 mid = new Category1 { Title = "Средний вес" };
        Category1 light = new Category1 { Title = "Легкий вес" };

        db.Categories.AddRange(high, mid, light);
        db.SaveChanges();

        Athlete1 a1 = new Athlete1 { FIO = "Леннокс Льюис", wins = 41, lose = 2, year = 1989, Category = high };
        Athlete1 a2 = new Athlete1 { FIO = "Макс Тайсон", wins = 50, lose = 6, year = 1985, Category = high};
        Athlete1 a3 = new Athlete1 { FIO = "Виталий Кличко", wins = 45, lose = 2, year = 1996, Category = high };
        Athlete1 a4 = new Athlete1 { FIO = "Оскар Де Лахойя", wins = 45, lose = 6, year = 1992, Category = mid };
        Athlete1 a5 = new Athlete1 { FIO = "Бернард Хопкинс", wins = 55, lose = 8, year = 1988, Category = mid };
        Athlete1 a6 = new Athlete1 { FIO = "Джермин Тейлор", wins = 33, lose = 4, year = 2001, Category = mid };
        Athlete1 a7 = new Athlete1 { FIO = "Juan Manuel Márque", wins = 56, lose = 7, year = 1993, Category = light };
        Athlete1 a8 = new Athlete1 { FIO = "Nate Campbell", wins = 37, lose = 11, year = 2000, Category = light };
        Athlete1 a9 = new Athlete1 { FIO = "Humberto Soto", wins = 69, lose = 10, year = 1997, Category = light };
        // для запросов, тестов
        Athlete1 a10 = new Athlete1 { FIO = "1", wins = 9, lose = 5, year = 2019, Category = light };
        Athlete1 a11 = new Athlete1 { FIO = "2", wins = 10, lose = 5, year = 2020, Category = mid };
        Athlete1 a12 = new Athlete1 { FIO = "3", wins = 3, lose = 8, year = 2020, Category = mid };
        Athlete1 a13 = new Athlete1 { FIO = "4", wins = 11, lose = 3, year = 2020, Category = high };
        Athlete1 a14 = new Athlete1 { FIO = "5", wins = 8, lose = 1, year = 2020, Category = high };

        db.Athletes.AddRange(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14);
        db.SaveChanges();
    }
}*/