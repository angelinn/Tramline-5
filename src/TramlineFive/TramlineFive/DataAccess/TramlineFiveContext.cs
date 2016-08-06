using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;

using SettingsEntity = TramlineFive.DataAccess.Entities.Settings;

namespace TramlineFive.DataAccess
{
    public class TramlineFiveContext : DbContext
    {
        public DbSet<Line> Lines { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Stop> Stops { get; set; }

        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<SettingsEntity> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={databaseName}");
        }

        private static string databaseName = "TramlneFive.db";
        public static string DatabaseName
        {
            get
            {
                return databaseName;
            }
        }
    }
}
