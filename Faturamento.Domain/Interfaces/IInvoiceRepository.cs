using Faturamento.Domain.Entities;

namespace Faturamento.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        Task AdicionarAsync(Invoice invoice);
        Task<int> ObterUltimoNumeroAsync();
        Task<Invoice?> ObterPorIdAsync(int id);
        Task AtualizarAsync(Invoice invoice);
        Task<IEnumerable<Invoice>> ObterTodosAsync();
    }
}