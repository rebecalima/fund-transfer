using Microsoft.AspNetCore.Mvc;

namespace TransferenciaBancariaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferenciaController : ControllerBase
{
    private readonly ILogger<TransferenciaController> _logger;

    public TransferenciaController(ILogger<TransferenciaController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTransferencia")]
    public string Get()
    {
        _logger.LogInformation("Getting calcule");

        return "OK";
    }
}
