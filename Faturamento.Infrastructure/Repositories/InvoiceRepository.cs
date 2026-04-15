using Faturamento.Domain.Entities;
using Faturamento.Domain.Interfaces;
using Faturamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Faturamento.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly FaturamentoContext _context;

        public InvoiceRepository(FaturamentoContext context)
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
            var max = await _context.Invoices.AnyAsync()
                      ? await _context.Invoices.MaxAsync(x => x.NumeroSequencial)
                      : 0;
            return max;
        }

        public async Task<IEnumerable<Invoice>> ObterTodosAsync()
        {
            return await _context.Invoices.Include(x => x.Itens).ToListAsync();
        }

        public async Task<Invoice?> ObterPorIdAsync(int id)
        {
            return await _context.Invoices.Include(x => x.Itens).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AtualizarAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
    }
}