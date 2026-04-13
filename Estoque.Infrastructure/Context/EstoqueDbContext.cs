using Estoque.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infrastructure.Context
{
    public class EstoqueDbContext : DbContext
    {
        public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Codigo).IsRequired().HasMaxLength(50);
                entity.HasIndex(p => p.Codigo).IsUnique();
                entity.Property(p => p.Descricao).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Saldo).IsRequired();
            });
        }
    }
}