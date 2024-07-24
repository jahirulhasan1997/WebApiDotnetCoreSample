using Microsoft.EntityFrameworkCore;

namespace WebApiDotnetCoreSample.DataStoreModel
{
    public class PizzaDbContext :DbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Pizza> Pizza { get; set; }
    }
}
