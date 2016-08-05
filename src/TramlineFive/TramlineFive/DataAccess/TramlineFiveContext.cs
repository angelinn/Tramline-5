using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SettingsEntity = EasyBus.DataAccess.Entities.Settings;

namespace TramlineFive.DataAccess
{
    public class TramlineFiveContext : DbContext
    {
        public DbSet<SettingsEntity> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=TramlineFive.db");
        }
    }
}
