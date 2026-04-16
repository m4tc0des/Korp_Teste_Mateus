using System.Net.Http.Json;

public class EstoqueHttpClient
{
    private readonly HttpClient _httpClient;

    public EstoqueHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7265/");
    }

    public async Task<ProdutoEstoqueDto> ObterProdutoAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/products/{id}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<ProdutoEstoqueDto>();
    }

    public async Task<bool> BaixarEstoqueAsync(int produtoId, int quantidade)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/products/{produtoId}/baixar-estoque", new { Quantidade = quantidade });
        return response.IsSuccessStatusCode;
    }
}