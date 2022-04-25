using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Nornickel1.EF
{
    internal class DbAthContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Athlete> Athletes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS01;Database=hw_1_1;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
        public void CreateIfNotEx()
        {
            this.Database.EnsureCreated();
        }

        //@"Server=.\SQLEXPRESS01;Database=hw_1;Trusted_Connection=True;"
    }
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
    }
    public class Athlete
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string FIO { get; set; }
        public int wins { get; set; }
        public int lose { get; set; }
        public int year { get; set; }
        public Category Category { get; set; }
    }
}
