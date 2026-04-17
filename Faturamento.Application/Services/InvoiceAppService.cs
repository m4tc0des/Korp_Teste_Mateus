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
        if (invoiceDto.Itens == null || !invoiceDto.Itens.Any())
            throw new Exception("Não é possível gerar uma nota sem itens.");
        
        int proximoNumero = await _invoiceRepository.ObterUltimoNumeroAsync() + 1;

        var novaFatura = new Invoice();
        novaFatura.NumeroSequencial = proximoNumero;

        novaFatura.Status = Faturamento.Domain.Entities.Enums.InvoiceStatus.Aberta;

        foreach (var item in invoiceDto.Itens)
        {

            var produtoNoEstoque = await _estoqueClient.ObterProdutoAsync(item.ProdutoId);

            if (produtoNoEstoque == null)
                throw new Exception($"Produto ID {item.ProdutoId} não encontrado.");

            if (item.Quantidade > produtoNoEstoque.Saldo)
                throw new Exception($"Saldo insuficiente para o produto {produtoNoEstoque.Codigo}.");

            await _estoqueClient.BaixarEstoqueAsync(item.ProdutoId, item.Quantidade);

            novaFatura.AdicionarItem(item.ProdutoId, produtoNoEstoque.Codigo, item.Quantidade);
        }

        await _invoiceRepository.AdicionarAsync(novaFatura);
        return novaFatura.NumeroSequencial;
    }

    public async Task FecharNotaAsync(int notaId)
    {
        var fatura = await _invoiceRepository.ObterPorIdAsync(notaId);

        if (fatura == null)
            throw new Exception("Nota Fiscal não encontrada.");

        if (fatura.Status == Faturamento.Domain.Entities.Enums.InvoiceStatus.Fechada)
            throw new Exception("Esta nota já está fechada.");

        fatura.Status = Faturamento.Domain.Entities.Enums.InvoiceStatus.Fechada;
        await _invoiceRepository.AtualizarAsync(fatura);
    }

    public async Task<List<InvoiceDto>> ObterTodasAsync()
    {
        var faturas = await _invoiceRepository.ObterTodosAsync();

        return faturas.Select(x => new InvoiceDto
        {
            Id = x.Id,
            NumeroSequencial = x.NumeroSequencial,
            Status = x.Status,
            Itens = x.Itens.Select(i => new InvoiceItemDto
            {
                ProdutoId = i.ProdutoId,
                ProdutoCodigo = i.ProdutoCodigo,
                Quantidade = i.Quantidade
            }).ToList()
        }).ToList();
    }
}