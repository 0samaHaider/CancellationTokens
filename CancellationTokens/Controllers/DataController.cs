using Microsoft.AspNetCore.Mvc;

namespace CancellationTokens.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DataController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    [HttpGet("fetchDataWithoutCancellation")]
    public async Task<IActionResult> FetchDataWithoutCancellation()
    {
        try
        {
            // Simulate an external API endpoint
            var apiUrl = "https://jsonplaceholder.typicode.com/todos/1";

            // Create an HttpClient using the factory
            using var httpClient = _httpClientFactory.CreateClient();

            // Simulate a non-cancellable asynchronous API call
            var response = await httpClient.GetStringAsync(apiUrl);

            // Process the API response
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest($"API call failed: {ex.Message}");
        }
    }

    [HttpGet("fetchDataWithCancellation")]
    public async Task<IActionResult> FetchDataWithCancellation(CancellationToken cancellationToken)
    {
        try
        {
            // Simulate an external API endpoint
            var apiUrl = "https://jsonplaceholder.typicode.com/todos/1";

            // Create an HttpClient using the factory
            using var httpClient = _httpClientFactory.CreateClient();

            // Simulate an asynchronous API call with cancellation
            var response = await httpClient.GetStringAsync(apiUrl, cancellationToken);

            // Process the API response
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return BadRequest($"API call failed: {ex.Message}");
        }
        catch (OperationCanceledException)
        {
            return StatusCode(500, "API call was canceled.");
        }
    }
}
