using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infrastructure.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly EstoqueDbContext _context;

        public ProductRepository(EstoqueDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> ObterPorIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> ObterPorCodigoAsync(string codigo)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<IEnumerable<Product>> ObterTodosAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AdicionarAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
