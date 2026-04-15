
using Faturamento.Domain.Entities;

namespace Faturamento.Application.Interfaces
{
    public interface IInvoiceAppService
    {
        Task<int> GerarNotaAsync(CreateInvoiceDto invoiceDto);
        Task FecharNotaAsync(int notaId);
        Task<Invoice?> ObterPorIdAsync(int id);
        Task<List<CreateInvoiceDto>> ObterTodasAsync();
    }
}