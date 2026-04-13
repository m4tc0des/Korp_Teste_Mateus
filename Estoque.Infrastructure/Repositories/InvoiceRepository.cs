using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly EstoqueDbContext _context;

        public InvoiceRepository(EstoqueDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ObterUltimoNumeroAsync()
        {
            return await _context.Invoices
                .OrderByDescending(x => x.NumeroSequencial)
                .Select(x => x.NumeroSequencial)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> ObterPorIdAsync(int id)
        {
            return await _context.Invoices
                .Include(x => x.Itens)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}