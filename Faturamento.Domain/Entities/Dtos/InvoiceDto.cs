
public class InvoiceDto
{
    public int Id { get; set; }
    public DateTime DataCadastro { get; set; }
    public List<InvoiceItemDto> Itens { get; set; }
}