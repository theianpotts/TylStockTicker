using Microsoft.EntityFrameworkCore;
using TylStockTicker.Models;

namespace TylStockTicker.Data
{
    public class StockContext :DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<StockTransaction> StockTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockTransaction>().ToTable("StockTransaction");
        }
    }
}