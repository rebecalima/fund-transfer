using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TransferenciaBancariaAPI.Interface;
using TransferenciaBancariaAPI.Models;
using TransferenciaBancariaAPI.Services;

namespace TransferenciaBancariaAPI.Controllers;

[ApiController]
[Route("api/transfer")]
public class TransferenciaController : ControllerBase
{
    private readonly ILogger<TransferenciaController> _logger;

    public TransferenciaController(ILogger<TransferenciaController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "PostTransferencia")]
    public void Post([FromBody] Transferencia transferencia)
    {

    }
}
