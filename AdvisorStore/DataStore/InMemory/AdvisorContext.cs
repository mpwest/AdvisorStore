using Microsoft.EntityFrameworkCore;

namespace AdvisorStore
{
    public class AdvisorContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AdvisorDb");
        }

        public DbSet<Advisor> Advisors { get; set; }
    }
}