using System;
using System.Net.Http;
using System.Threading.Tasks;
using FundTransferAPI.Interface;
using FundTransferAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using Xunit;

namespace FundTransferAPI.Tests;

public class TransferServiceTest
{
    [Fact]
    public async void transferValueFromAccountOriginToAccountDestination_ReturnsTrue()
    {
        var acessoClientAPI = new Mock<IClientAPI>();
        acessoClientAPI.Setup(x => x.GetAccount("123")).Returns(Task.FromResult(new Mock<HttpResponseMessage>().Object));
        var loggerMock = new Mock<ILogger<TransferService>>();
        var elasticMock = new Mock<IElasticSearchService>();
        elasticMock.Setup(x => x.GetClient()).Returns(new Mock<IElasticClient>().Object);
        var transfer = new Transfer
        {
            Id = Guid.NewGuid(),
            AccountOrigin = "321",
            AccountDestination = "123",
            Value = 30,
            Status = StatusType.PROCESSING,
            Date = DateTime.UtcNow
        };
        var transferService = new TransferService(loggerMock.Object, elasticMock.Object, acessoClientAPI.Object);

        var result = await transferService.transferValue(transfer);

        Assert.Equal(true, result);
    }
}