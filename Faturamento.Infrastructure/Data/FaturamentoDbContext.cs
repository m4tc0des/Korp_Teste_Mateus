using Faturamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Faturamento.Infrastructure.Data
{
    public class FaturamentoContext : DbContext
    {
        public FaturamentoContext(DbContextOptions<FaturamentoContext> options) : base(options)
        {

        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().ToTable("Invoices");

            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItems");

            base.OnModelCreating(modelBuilder);
        }
    }

    public class FaturamentoContextFactory : IDesignTimeDbContextFactory<FaturamentoContext>
    {
        public FaturamentoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FaturamentoContext>();

            var connectionString = "Server=localhost;Database=FaturamentoDB;User=root;Password=88331614;";

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new FaturamentoContext(optionsBuilder.Options);
        }
    }

}
