using Estoque.Application.Dtos;
using Estoque.Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
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
            await _productAppService.CadastrarProdutoAsync(dto);

            return Ok(new { message = "Produto cadastrado com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet] 
    public async Task<IActionResult> ObterTodos()
    {
        var produtos = await _productAppService.ObterTodosAsync();
        return Ok(produtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produto = await _productAppService.ObterPorIdAsync(id);
        if (produto == null) return NotFound();

        return Ok(new
        {
            Id = produto.Id,
            Codigo = produto.Codigo,
            Saldo = produto.Saldo
        });
    }

    [HttpPost("{id:int}/baixar-estoque")]
    public async Task<IActionResult> BaixarProduto(int id, [FromBody] BaixaEstoqueRequest request)
    {
        try
        {
            await _productAppService.BaixarEstoquePorIdAsync(id, request.Quantidade);
            return Ok(new { message = "Baixa realizada com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}