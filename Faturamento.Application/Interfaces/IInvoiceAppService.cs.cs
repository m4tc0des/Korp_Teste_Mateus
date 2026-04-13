
using Faturamento.Domain.Entities;
using Faturamento.Domain.Entities.Dtos;

namespace Faturamento.Application.Interfaces
{
    public interface IInvoiceAppService
    {
        Task<int> GerarNotaAsync(CreateInvoiceDto dto);
        Task FecharNotaAsync(int notaId);
        Task<Invoice?> ObterPorIdAsync(int id);
    }
}