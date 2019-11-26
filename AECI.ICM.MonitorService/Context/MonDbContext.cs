using MonitorService.Entities;
using System.Data.Entity;

namespace MonitorService
{
    public class MonDbContext:DbContext
    {
        public MonDbContext():base("MonitorContext")
        {
        }

        public DbSet<Setting> Setting { get; set; }

        public DbSet<SettingEmail> SettingEmails { get; set; }

        public DbSet<Result> Results { get; set; }
    }
}
