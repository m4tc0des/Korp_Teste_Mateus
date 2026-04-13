using Estoque.Application.Dtos;
using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;

namespace Estoque.Application.Services
{
    public class ProductAppService
    {
        private readonly IProductRepository _repository;

        public ProductAppService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CadastrarProdutoAsync(CreateProductDto dto)
        {
            var produto = new Product(dto.Codigo, dto.Descricao, dto.Saldo);
            await _repository.AdicionarAsync(produto);
            return produto.Id;
        }

        public async Task BaixarEstoqueAsync(string codigo, int quantidade)
        {
            var produto = await _repository.ObterPorCodigoAsync(codigo);
            if (produto == null) throw new Exception("Produto não encontrado.");

            produto.DebitarEstoque(quantidade);
            await _repository.AtualizarAsync(produto);
        }

        public async Task<Product?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id);
        }
    }
}