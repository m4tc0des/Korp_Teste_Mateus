using Estoque.Application.Dtos;
using Estoque.Domain.Entities;
using Estoque.Domain.Entities.Enums;
using Estoque.Domain.Interfaces;
using Estoque.Infrastructure.Repositories;

namespace Estoque.Application.Services
{
    public class InvoiceAppService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;
        public InvoiceAppService(IInvoiceRepository invoiceRepository, IProductRepository productRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
        }

        public async Task<int> GerarNotaAsync(CreateInvoiceDto dto)
        {
            if (dto.Itens == null || !dto.Itens.Any())
                throw new Exception("A nota precisa ter pelo menos um item.");

            foreach (var item in dto.Itens)
            {
                if (item.Quantidade <= 0)
                    throw new Exception("Não é possível gerar uma nota com item de quantidade zero.");

                if (string.IsNullOrWhiteSpace(item.ProdutoCodigo) || item.ProdutoCodigo == "string")
                    throw new Exception("Código de produto inválido.");
            }

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

        public async Task FecharNotaAsync(int notaId)
        {
            var nota = await _invoiceRepository.ObterPorIdAsync(notaId);

            if (nota == null)
                throw new Exception("Nota Fiscal não encontrada.");

            if (nota.Status == InvoiceStatus.Fechada)
                throw new Exception("Esta nota já está fechada e não pode ser impressa novamente.");

            foreach (var item in nota.Itens)
            {
                var produto = await _productRepository.ObterPorCodigoAsync(item.ProdutoCodigo);

                if (produto == null)
                    throw new Exception($"Falha na Recuperação: Produto {item.ProdutoCodigo} não encontrado no Serviço de Estoque.");

                if (produto.Saldo < item.Quantidade)
                    throw new Exception($"Saldo insuficiente para o produto {produto.Descricao}. Saldo atual: {produto.Saldo}");

                produto.Saldo -= item.Quantidade;
                await _productRepository.AtualizarAsync(produto);
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