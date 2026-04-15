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

            if (string.IsNullOrWhiteSpace(dto.Codigo))
            {
                throw new Exception("O código do produto não pode estar vazio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Descricao))
            {
                throw new Exception("A descrição do produto não pode estar vazia.");
            }

            if (dto.Saldo < 0)
            {
                throw new Exception("O saldo inicial não pode ser negativo.");
            }

            var produto = new Product(dto.Codigo, dto.Descricao, dto.Saldo);
            await _productRepository.AdicionarAsync(produto);
            return produto.Id;
        }

        public async Task<Product?> ObterPorCodigoAsync(string codigo)
        {
            return await _productRepository.ObterPorCodigoAsync(codigo);
        }

        public async Task BaixarEstoquePorIdAsync(int id, int quantidade)
        {
            var produto = await _productRepository.ObterPorIdAsync(id);

            if (produto == null)
                throw new Exception("Produto não encontrado no estoque.");

            produto.DebitarEstoque(quantidade);
            await _productRepository.AtualizarAsync(produto);
        }

        public async Task<Product?> ObterPorIdAsync(int id)
        {
            return await _productRepository.ObterPorIdAsync(id);
        }
    }
}