using Estoque.Domain.Entities;

namespace Estoque.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> ObterPorIdAsync(int id);
        Task<Product?> ObterPorCodigoAsync(string codigo);
        Task<IEnumerable<Product>> ObterTodosAsync();
        Task AdicionarAsync(Product produto);
        Task AtualizarAsync(Product produto);
    }
}
