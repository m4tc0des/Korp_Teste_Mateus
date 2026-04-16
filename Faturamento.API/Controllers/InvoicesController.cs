using Faturamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceAppService _invoiceAppService;

    public InvoiceController(IInvoiceAppService invoiceAppService)
    {
        _invoiceAppService = invoiceAppService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarNota([FromBody] CreateInvoiceDto dto)
    {
        try
        {
            var numeroNota = await _invoiceAppService.GerarNotaAsync(dto);
            return Ok(new { message = $"Nota Fiscal nº {numeroNota} criada com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodasNotas()
    {
        var notas = await _invoiceAppService.ObterTodasAsync();
        return Ok(notas);
    }
}