using Faturamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
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
            var num = await _invoiceAppService.GerarNotaAsync(dto);
            return Ok(new { message = $"Nota {num} criada (Aberta)." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _invoiceAppService.ObterTodasAsync());

    [HttpPost("fecharNota/{id}")]
    public async Task<IActionResult> Fechar(int id)
    {
        try
        {
            await _invoiceAppService.FecharNotaAsync(id);
            return Ok(new { message = "Nota finalizada e estoque atualizado!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}