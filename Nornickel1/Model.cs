namespace AdoEfDemo;
public class Category
{
    public int Id;
    public string Title;
}
public class Athlete
{
    public int Id;
    public string FIO;
    public int wins;
    public int lose;
    public int year;
    public Category Category;
}
public class Database
{
    public List<Category> Categories;
    public List<Athlete> Athletes;
    public Database()
    {
        Categories = new List<Category>{
            new Category { Id = 1, Title = "Тяжелый вес" },
            new Category { Id = 2, Title = "Средний вес"},
            new Category { Id = 3, Title = "Легкий вес"}
            };
        Athletes = new List<Athlete>{
                new Athlete { Id = 1,FIO = "Леннокс Льюис", wins = 41, lose = 2, year = 1989, Category = Categories[0]},
                new Athlete { Id = 2,FIO = "Макс Тайсон", wins = 50, lose = 6, year = 1985, Category = Categories[0]},
                new Athlete { Id = 3,FIO = "Виталий Кличко", wins = 45, lose = 2, year = 1996, Category = Categories[0]},
                new Athlete { Id = 4,FIO = "Оскар Де Лахойя", wins = 45, lose = 6, year = 1992, Category = Categories[1]},
                new Athlete { Id = 5,FIO = "Бернард Хопкинс", wins = 55, lose = 8, year = 1988, Category = Categories[1]},
                new Athlete { Id = 6,FIO = "Джермин Тейлор", wins = 33, lose = 4, year = 2001, Category = Categories[1]},
                new Athlete { Id = 7,FIO = "Juan Manuel Márque", wins = 56, lose = 7, year = 1993, Category = Categories[2]},
                new Athlete { Id = 8,FIO = "Nate Campbell", wins = 37, lose = 11, year = 2000, Category = Categories[2]},
                new Athlete { Id = 9,FIO = "Humberto Soto", wins = 69, lose = 10, year = 1997, Category = Categories[2]},
                // для запросов, тестов
                new Athlete { Id = 10,FIO = "1", wins = 9, lose = 5, year = 2019, Category = Categories[2]},
                new Athlete { Id = 11,FIO = "2", wins = 10, lose = 5, year = 2020, Category = Categories[1]},
                new Athlete { Id = 12,FIO = "3", wins = 3, lose = 8, year = 2020, Category = Categories[1]},
                new Athlete { Id = 13,FIO = "4", wins = 11, lose = 3, year = 2020, Category = Categories[0]},
                new Athlete { Id = 14,FIO = "5", wins = 8, lose = 1, year = 2020, Category = Categories[0]},

            };
    }

}

