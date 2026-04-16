
namespace Faturamento.Application.Interfaces
{
    public interface IInvoiceAppService
    {
        Task<int> GerarNotaAsync(CreateInvoiceDto invoiceDto);
        Task FecharNotaAsync(int notaId);
        Task<List<InvoiceDto>> ObterTodasAsync();
    }
}