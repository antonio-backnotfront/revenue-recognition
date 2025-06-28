namespace RevenueRecognition.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Client;
using Application.Services.Client;

[ApiController]
[Route("/api/clients")]
public class ClientController : ControllerBase
{
    private readonly ILogger<ClientController> _logger;
    private readonly IClientService _service;

    public ClientController(
        ILogger<ClientController> logger,
        IClientService service
    )
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClientsAsync(
        CancellationToken cancellationToken
    )
    {
        return Ok(await _service.GetClientsAsync(cancellationToken));
    }

    [HttpGet("{id}"), ActionName("GetClientByIdOrThrowAsync")]
    public async Task<IActionResult> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        GetClientResponse? response = await _service.GetClientByIdOrThrowAsync(id, cancellationToken);
        return response != null
            ? StatusCode(200, await _service.GetClientByIdOrThrowAsync(id, cancellationToken))
            : StatusCode(404, new
            {
                type = $"https://httpstatuses.com/404",
                title = $"Client with id {id} not found.",
                status = 404,
            });
    }

    [HttpPost]
    public async Task<IActionResult> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken
    )
    {
        GetClientResponse createdClient = await _service.CreateClientOrThrowAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetClientByIdAsync), new { Id = createdClient.Id }, createdClient);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateClientAsync(
        int id,
        UpdateClientDto dto,
        CancellationToken cancellationToken
    )
    {
        GetClientResponse updatedClient = await _service.UpdateClientByIdOrThrowAsync(id, dto, cancellationToken);
        return Ok(updatedClient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> UpdateClientAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        await _service.RemoveClientByIdOrThrowAsync(id, cancellationToken);
        return NoContent();
    }
}