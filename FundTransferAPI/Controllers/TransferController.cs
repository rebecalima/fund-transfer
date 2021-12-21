using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using FundTransferAPI.Interface;
using FundTransferAPI.Models;
using Nest;

namespace FundTransferAPI.Controllers;

[ApiController]
[Route("api/fund-transfer")]
public class TransferController : ControllerBase
{
    private readonly ILogger<TransferController> _logger;
    private readonly IElasticClient _elasticClient;
    private readonly IProducerService _producer;

    public TransferController(
        ILogger<TransferController> logger,
        IElasticSearchService elasticClient,
         IProducerService producer)
    {
        _logger = logger;
        _elasticClient = elasticClient.GetClient();
        _producer = producer;
    }

    [HttpGet(Name = "GetTransferStatus")]
    public IActionResult Get(Guid id)
    {
        var doc = _elasticClient.Get<Transfer>(id).Source;

        if (doc is null)
            return NotFound();


        if (doc.Status == StatusType.ERROR)
            return Ok(new
            {
                Status = doc.Status,
                Message = doc.Error
            });

        return Ok(new { Status = doc.Status });
    }

    [HttpPost(Name = "PostTransfer")]
    public IActionResult Post([FromBody] TransferBase transferBase)
    {
        if (transferBase is null)
            return BadRequest("Information about transfer is required.");

        var transfer = new Transfer(transferBase);
        _elasticClient.Index(transfer, idx => idx.Index("transfer"));
        var transferSerialized = JsonSerializer.Serialize(transfer);

        IMessage message = new Message(transferSerialized);

        string queue = "account-transfer-pending";
        _producer.enqueue(message, queue);

        _logger.LogInformation($"A new message was published on queue {queue}");

        return Ok(new { transactionId = transfer.Id });

    }
}
