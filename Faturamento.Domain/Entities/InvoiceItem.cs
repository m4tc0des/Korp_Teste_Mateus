namespace Faturamento.Domain.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public string ProdutoCodigo { get; set; }

        public int Quantidade { get; set; }

        public InvoiceItem(string produtoCodigo, int quantidade)
        {
            ProdutoCodigo = produtoCodigo;
            Quantidade = quantidade;
        }

        protected InvoiceItem() { }
    }
}