
using Faturamento.Application.Interfaces;
using Faturamento.Domain.Entities;
using Faturamento.Domain.Entities.Dtos;
using Faturamento.Domain.Entities.Enums;
using Faturamento.Domain.Interfaces;
using System.Net.Http.Json;

namespace Faturamento.Application.Services
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly HttpClient _httpClient;

        public class ProductDto
        {
            public string Codigo { get; set; }
            public int Saldo { get; set; }
        }

        public InvoiceAppService(IInvoiceRepository invoiceRepository, IHttpClientFactory httpClientFactory)
        {
            _invoiceRepository = invoiceRepository;
            _httpClient = httpClientFactory.CreateClient("EstoqueAPI");
        }

        public async Task<int> GerarNotaAsync(CreateInvoiceDto dto)
        {
            if (dto.Itens == null || !dto.Itens.Any())
                throw new Exception("A nota precisa ter pelo menos um item.");

            var ultimoNumero = await _invoiceRepository.ObterUltimoNumeroAsync();

            var novaNota = new Invoice
            {
                NumeroSequencial = ultimoNumero + 1,
                Status = InvoiceStatus.Aberta
            };

            foreach (var item in dto.Itens)
            {
                novaNota.AdicionarItem(item.ProdutoCodigo, item.Quantidade);
            }

            await _invoiceRepository.AdicionarAsync(novaNota);
            return novaNota.NumeroSequencial;
        }

        public async Task FecharNotaAsync(int notaId)
        {
            var nota = await _invoiceRepository.ObterPorIdAsync(notaId);

            if (nota == null) throw new Exception("Nota Fiscal não encontrada.");
            if (nota.Status == InvoiceStatus.Fechada) throw new Exception("Esta nota já está fechada.");

            foreach (var item in nota.Itens)
            {
                var response = await _httpClient.GetAsync($"api/product/{item.ProdutoCodigo}");
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Produto {item.ProdutoCodigo} não encontrado.");

                var produto = await response.Content.ReadFromJsonAsync<ProductDto>();
                if (produto == null || produto.Saldo < item.Quantidade)
                    throw new Exception($"Saldo insuficiente para o produto {item.ProdutoCodigo}.");

                var baixaResponse = await _httpClient.PostAsJsonAsync($"api/product/{item.ProdutoCodigo}/baixar-estoque", new { quantidade = item.Quantidade });
                if (!baixaResponse.IsSuccessStatusCode)
                    throw new Exception($"Erro ao baixar estoque do produto {item.ProdutoCodigo}.");
            }

            nota.Status = InvoiceStatus.Fechada;
            await _invoiceRepository.AtualizarAsync(nota);
        }

        public async Task<Invoice?> ObterPorIdAsync(int id)
        {
            return await _invoiceRepository.ObterPorIdAsync(id);
        }
    }
}