// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
using AdoEfDemo;
using Nornickel1.EF;

static void AdoNetDemo()
{
    var connection = new SqlConnection(@"Server=.\SQLEXPRESS01;Database=hw_1;Trusted_Connection=True;");
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM Category";
    var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine($"Title = {reader.GetString(reader.GetOrdinal("Title"))}");
    }
    reader.Close();
    connection.Close();
}
//AdoNetDemo();

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

