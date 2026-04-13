using Estoque.Application.Dtos;
using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;

namespace Estoque.Application.Services
{
    public class InvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceAppService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<int> GerarNotaAsync(CreateInvoiceDto dto)
        {
            var ultimoNumero = await _invoiceRepository.ObterUltimoNumeroAsync();

            var novaNota = new Invoice
            {
                NumeroSequencial = ultimoNumero + 1,
            };

            foreach (var item in dto.Itens)
            {
                novaNota.AdicionarItem(item.ProdutoCodigo, item.Quantidade);
            }

            await _invoiceRepository.AdicionarAsync(novaNota);
            return novaNota.NumeroSequencial;
        }
    }
}