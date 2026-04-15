using Faturamento.Domain.Entities.Enums;

namespace Faturamento.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public int NumeroSequencial { get; set; }
        public InvoiceStatus Status { get; set; }
        public List<InvoiceItem> Itens { get; set; } = new();

        public Invoice()
        {
            Status = InvoiceStatus.Aberta;
        }

        public void AdicionarItem(int produtoId, string produtoCodigo, int quantity)
        {
            if (Status == InvoiceStatus.Fechada)
                throw new InvalidOperationException("Não é possível adicionar itens a uma nota fechada.");

            Itens.Add(new InvoiceItem(produtoId, produtoCodigo, quantity));
        }
    }
}