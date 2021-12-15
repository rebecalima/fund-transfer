using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TransferenciaBancariaAPI.Interface;
using TransferenciaBancariaAPI.Models;
using TransferenciaBancariaAPI.Services;

namespace TransferenciaBancariaAPI.Controllers;

[ApiController]
[Route("api/transfer/pending")]
public class TransferenciaPendingController : ControllerBase
{
    private readonly ILogger<TransferenciaPendingController> _logger;

    public TransferenciaPendingController(ILogger<TransferenciaPendingController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTransferencia")]
    public string Get()
    {
        _logger.LogError("Hey");

        return "OK";
    }

    [HttpPost(Name = "PostTransferenciaMessage")]
    public IActionResult Post([FromServices] IMessageService service, [FromBody] Transferencia transferencia)
    {
        if (transferencia is not null)
        {
            var elasticClient = new ElasticSearchService();
            elasticClient._client.Index(transferencia, idx => idx.Index("transferencia"));
            var transferenciaJson = JsonSerializer.Serialize(transferencia);
            IMessage message = new Message(transferenciaJson);
            string queue = "account-transfer-pending";
            var producer = new ProducerService(service);
            producer.enqueue(message, queue);
            _logger.LogInformation($"A new message was published on queue {queue}");

            return Ok(transferenciaJson);
        }
        return BadRequest();
    }
}
