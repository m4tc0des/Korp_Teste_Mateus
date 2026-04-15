using Faturamento.Application.Interfaces;
using Faturamento.Domain.Entities;
using Faturamento.Domain.Interfaces;

public class InvoiceAppService : IInvoiceAppService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly EstoqueHttpClient _estoqueClient;

    public InvoiceAppService(IInvoiceRepository invoiceRepository, EstoqueHttpClient estoqueClient)
    {
        _invoiceRepository = invoiceRepository;
        _estoqueClient = estoqueClient;
    }

    public async Task<int> GerarNotaAsync(CreateInvoiceDto invoiceDto)
    {
        foreach (var item in invoiceDto.Itens)
        {
            var produtoNoEstoque = await _estoqueClient.ObterProdutoAsync(item.ProdutoId);

            if (produtoNoEstoque == null)
                throw new Exception($"Produto {item.ProdutoId} não existe no Estoque.");

            if (produtoNoEstoque.Saldo < item.Quantidade)
            {
                throw new Exception($"Estoque insuficiente para o produto {item.ProdutoCodigo}. " +
                                    $"Disponível: {produtoNoEstoque.Saldo}, Pedido: {item.Quantidade}");
            }
        }

        var novaFatura = new Invoice();
        novaFatura.NumeroSequencial = invoiceDto.NumeroSequencial;

        await _invoiceRepository.AdicionarAsync(novaFatura);
        return novaFatura.Id;
    }

    public async Task FecharNotaAsync(int notaId)
    {
        var fatura = await _invoiceRepository.ObterPorIdAsync(notaId);
        if (fatura == null) throw new Exception("Nota não encontrada.");

        await _invoiceRepository.AtualizarAsync(fatura);
    }

    public async Task<Invoice?> ObterPorIdAsync(int id)
    {
        return await _invoiceRepository.ObterPorIdAsync(id);
    }

    public async Task<List<CreateInvoiceDto>> ObterTodasAsync()
    {
        var faturas = await _invoiceRepository.ObterTodosAsync();
        return faturas.Select(f => new CreateInvoiceDto { NumeroSequencial = f.NumeroSequencial }).ToList();
    }
}