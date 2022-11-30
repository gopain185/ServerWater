using Microsoft.EntityFrameworkCore;
namespace ServerWater.Model
{
    public class DataContext : DbContext
    {
        public static Random random = new Random();

        public static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public DbSet<SqlUser>? users { get; set; }
        public DbSet<SqlRole>? roles { get; set; }
        public DbSet<SqlArea>? areas { get; set; }
        public DbSet<SqlCustomer>? customers { get; set; }
        public DbSet<SqlAction>? sections { get; set; }

        public static string configSql = "Host=office.stvg.vn:59052;Database=db_smartwater;Username=stvg;Password=stvg";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(configSql);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SqlArea>().HasMany<SqlCustomer>(s => s.customers).WithOne(s => s.area);
            modelBuilder.Entity<SqlArea>().HasMany<SqlUser>(s => s.users).WithMany(s => s.areas);
        }
    }
}
