using Estoque.Application.Dtos;
using Estoque.Domain.Entities;
using Estoque.Domain.Interfaces;

namespace Estoque.Application.Services
{
    public class ProductAppService
    {
        private readonly IProductRepository _productRepository;

        public ProductAppService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> ObterTodosAsync()
        {
            return await _productRepository.ObterTodosAsync();
        }
        public async Task<int> CadastrarProdutoAsync(CreateProductDto dto)
        {
            var produto = new Product(dto.Codigo, dto.Descricao, dto.Saldo);
            await _productRepository.AdicionarAsync(produto);
            return produto.Id;
        }

        public async Task BaixarEstoqueAsync(string codigo, int quantidade)
        {
            var produto = await _productRepository.ObterPorCodigoAsync(codigo);
            if (produto == null) throw new Exception("Produto não encontrado.");

            produto.DebitarEstoque(quantidade);
            await _productRepository.AtualizarAsync(produto);
        }

        public async Task<Product?> ObterPorIdAsync(int id)
        {
            return await _productRepository.ObterPorIdAsync(id);
        }


    }
}