using Estoque.Application.Dtos;
using Estoque.Application.Services;
using Estoque.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductAppService _productAppService;

        public ProductsController(ProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto([FromBody] CreateProductDto dto)
        {
            try
            {
                var novoId = await _productAppService.CadastrarProdutoAsync(dto);

                return CreatedAtAction(nameof(LerProdutoPorId), new { id = novoId }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> LerTodosOsProdutos()
        {
            var produtos = await _productAppService.ObterTodosAsync();
            return Ok(produtos);
        }

        [HttpPut("{codigo}/baixar-estoque")]
        public async Task<IActionResult> BaixarProduto(string codigo, [FromQuery] int quantidade)
        {
            try
            {
                await _productAppService.BaixarEstoqueAsync(codigo, quantidade);
                return Ok(new { message = $"Estoque do produto {codigo} atualizado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> LerProdutoPorId(int id)
        {

            var produtoDto = await _productAppService.ObterPorIdAsync(id);

            if (produtoDto == null)
            {
                return NotFound(new { message = $"Produto com ID {id} não encontrado." });
            }

            return Ok(produtoDto);
        }
    }
}