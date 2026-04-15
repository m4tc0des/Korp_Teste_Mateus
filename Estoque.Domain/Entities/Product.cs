namespace Estoque.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int Saldo { get; set; }

        public Product()
        {

        }
        public Product(string codigo, string descricao, int saldoInicial)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("O código do produto é obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("A descrição do produto é obrigatória.");

            if (saldoInicial < 0)
                throw new ArgumentException("O saldo inicial não pode ser negativo.");

            Codigo = codigo;
            Descricao = descricao;
            Saldo = saldoInicial;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new InvalidOperationException("A quantidade de saída deve ser maior que zero.");

            if (Saldo < quantidade)
                throw new InvalidOperationException($"Saldo insuficiente para o produto {Codigo}. Disponível: {Saldo}");

            Saldo -= quantidade;
        }

        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new InvalidOperationException("A quantidade de entrada deve ser maior que zero.");

            Saldo += quantidade;
        }
    }
}
