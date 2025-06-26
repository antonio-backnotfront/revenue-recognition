using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Application.DTOs.Client;
using RevenueRecognition.Application.Services.Client;

namespace RevenueRecognition.API.Controllers;

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

    [HttpGet("{id}"), ActionName("GetClientByIdAsync")]
    public async Task<IActionResult> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        GetClientResponse? response = await _service.GetClientByIdAsync(id, cancellationToken);
        return response != null
            ? StatusCode(200, await _service.GetClientByIdAsync(id, cancellationToken))
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
        CreateClientRequest? createdClient = await _service.CreateClientAsync(request, cancellationToken);
        if (createdClient != null)
        {
            return CreatedAtAction(nameof(GetClientByIdAsync), new { Id = createdClient.Id }, createdClient);
        }
        return StatusCode(500, new
        {
            type="https://httpstatuses.com/500",
            title="Could not create client",
            status=500 
        });
    }
}