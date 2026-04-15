using Faturamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Faturamento.API.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoicesController(IInvoiceAppService invoiceAppService)
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

        [HttpPost("{id}/imprimir")]
        public async Task<IActionResult> ImprimirNota(int id)
        {
            try
            {
                await _invoiceAppService.FecharNotaAsync(id);

                var notaFiscal = await _invoiceAppService.ObterPorIdAsync(id);

                return Ok(notaFiscal);
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
}