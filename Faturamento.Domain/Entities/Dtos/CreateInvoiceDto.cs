using Faturamento.Domain.Entities.Enums;

public class CreateInvoiceDto
{
    public int Id { get; set; }
    public int NumeroSequencial { get; set; }

    public InvoiceStatus Status { get; set; }

    public List<InvoiceItemDto> Itens { get; set; } = new();
}