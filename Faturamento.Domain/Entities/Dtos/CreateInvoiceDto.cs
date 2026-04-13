using System.Collections.Generic;

namespace Faturamento.Domain.Entities.Dtos
{
    public class CreateInvoiceDto
    {
        public List<InvoiceItemDto> Itens { get; set; } = new();
    }

    public class InvoiceItemDto
    {
        public string ProdutoCodigo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}