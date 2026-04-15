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
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Codigo).IsRequired().HasMaxLength(50);
                entity.HasIndex(x => x.Codigo).IsUnique();
                entity.Property(x => x.Descricao).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Saldo).IsRequired();
            });
        }
    }
}