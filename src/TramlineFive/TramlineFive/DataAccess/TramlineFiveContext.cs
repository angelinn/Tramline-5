using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using Windows.Storage;

namespace TramlineFive.DataAccess
{
    public class TramlineFiveContext : DbContext
    {
        public DbSet<Line> Lines { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Stop> Stops { get; set; }

        public DbSet<Favourite> Favourites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={ApplicationData.Current.LocalFolder.Path}\\{databaseName}");
        }

        private static string databaseName = "TramlineFive.db";
        public static string DatabaseName
        {
            get
            {
                return databaseName;
            }
        }
    }
}
