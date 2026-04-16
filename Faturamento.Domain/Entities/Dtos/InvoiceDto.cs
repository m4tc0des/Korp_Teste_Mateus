using Faturamento.Domain.Entities.Enums;

public class InvoiceDto
{
    public int Id { get; set; }
    public int NumeroSequencial { get; set; }
    public InvoiceStatus Status { get; set; }
    public List<InvoiceItemDto> Itens { get; set; }
}