namespace Estoque.Application.Dtos
{
    public class CreateProductDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int Saldo { get; set; }
    }

    public class BaixaEstoqueRequest
    {
        public int Quantidade { get; set; }
    }
}