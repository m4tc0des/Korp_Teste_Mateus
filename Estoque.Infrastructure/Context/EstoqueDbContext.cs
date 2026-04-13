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
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

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

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.NumeroSequencial).IsRequired();
                entity.HasIndex(i => i.NumeroSequencial).IsUnique();

                entity.Property(i => i.Status)
                    .HasConversion<string>()
                    .IsRequired();

                entity.HasMany(i => i.Itens)
                      .WithOne()
                      .HasForeignKey("InvoiceId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.ToTable("InvoiceItems");
                entity.HasKey(ii => ii.Id);
                entity.Property(ii => ii.ProdutoCodigo).IsRequired().HasMaxLength(50);
                entity.Property(ii => ii.Quantidade).IsRequired();
            });
        }
    }
}