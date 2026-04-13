namespace Estoque.Application.Dtos
{
    public class CreateInvoiceDto
    {
        public List<InvoiceItemDto> Itens { get; set; }
    }

    public class InvoiceItemDto
    {
        public string ProdutoCodigo { get; set; }
        public int Quantidade { get; set; }
    }
}