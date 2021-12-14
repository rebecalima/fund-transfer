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

    [HttpGet(Name = "GetTransferencia")]
    public string Get()
    {
        _logger.LogError("Hey");

        return "OK";
    }

    [HttpPost(Name = "PostTransferencia")]
    public void Post([FromServices] IMessageService service)
    {
        IMessage message = new Message("An new message.");
        string queue = "account-transfer-pending";
        var producer = new ProducerService(service);
        producer.enqueue(message, queue);
        _logger.LogInformation($"A new message was published on queue {queue}");
    }
}
