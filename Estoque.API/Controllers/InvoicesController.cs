using Estoque.Application.Dtos;
using Estoque.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceAppService _invoiceAppService;

        public InvoicesController(InvoiceAppService invoiceAppService)
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
    }
}