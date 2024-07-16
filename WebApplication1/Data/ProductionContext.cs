using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ProductionContext : DbContext
    {
        public ProductionContext(DbContextOptions<ProductionContext> options) : base(options)
        {
        }

        public DbSet<ProductionPlanComplete> ProductionPlansComplete { get; set; }
        public DbSet<ProductionResult> ProductionResults { get; set; }
    }
}
