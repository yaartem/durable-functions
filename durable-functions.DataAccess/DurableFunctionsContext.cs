using Microsoft.EntityFrameworkCore;

namespace durable_functions.DataAccess
{
    public class DurableFunctionsContext : DbContext 
    {
        public DbSet<ProcessDto> Processes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=durable-functions.db");
        }
    }
}