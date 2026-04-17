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

            var produtoExistente = await _productRepository.ObterPorCodigoAsync(dto.Codigo);
            if (produtoExistente != null)
            {
                throw new Exception($"O código '{dto.Codigo}' já está cadastrado em outro produto.");
            }

            if (string.IsNullOrWhiteSpace(dto.Codigo)) throw new Exception("Código obrigatório.");
            if (string.IsNullOrWhiteSpace(dto.Descricao)) throw new Exception("Descrição obrigatória.");

            try
            {
                var produto = new Product(dto.Codigo, dto.Descricao, dto.Saldo);

                await _productRepository.AdicionarAsync(produto);
                return produto.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro técnico ao salvar no banco: {ex.Message}");
            }
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
            await _productRepository.AtualizarAsync(produto);
        }

        public async Task<Product?> ObterPorIdAsync(int id)
        {
            return await _productRepository.ObterPorIdAsync(id);
        }
    }
}