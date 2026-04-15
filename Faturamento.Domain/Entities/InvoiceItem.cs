namespace Faturamento.Domain.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoCodigo { get; set; }
        public int Quantidade { get; set; }

        public InvoiceItem(int produtoId, string produtoCodigo, int quantidade)
        {
            ProdutoId = produtoId;
            ProdutoCodigo = produtoCodigo;
            Quantidade = quantidade;
        }

        protected InvoiceItem() { }
    }
}