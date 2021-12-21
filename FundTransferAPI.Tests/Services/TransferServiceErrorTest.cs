using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FundTransferAPI.Interface;
using FundTransferAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using Xunit;

namespace FundTransferAPI.Tests;

public class TransferServiceErrorTest
{
    [Fact]
    public async void transferValue_accountNotExists_ThrowsExceptionWithMessage()
    {
        var acessoClientAPI = new Mock<IClientAPI>();
        acessoClientAPI.Setup(x => x.GetAccount("123")).Returns(
            Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            }));
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

        await transferService.transferValue(transfer);

        Assert.Equal(StatusType.ERROR, transfer.Status);
        Assert.Equal(transfer.Error, $"The account destination {transfer.AccountDestination} not exists.");
    }

    [Fact]
    public async void transferValue_accountDestinationIsNULL_ThrowsExceptionWithMessage()
    {
        var acessoClientAPI = new Mock<IClientAPI>();
        var loggerMock = new Mock<ILogger<TransferService>>();
        var elasticMock = new Mock<IElasticSearchService>();
        elasticMock.Setup(x => x.GetClient()).Returns(new Mock<IElasticClient>().Object);

        var transfer = new Transfer
        {
            Id = Guid.NewGuid(),
            AccountOrigin = "321",
            Value = 30,
            Status = StatusType.PROCESSING,
            Date = DateTime.UtcNow
        };

        var transferService = new TransferService(loggerMock.Object, elasticMock.Object, acessoClientAPI.Object);

        await transferService.transferValue(transfer);

        Assert.Equal(StatusType.ERROR, transfer.Status);
        Assert.Equal(transfer.Error, $"The account destination {transfer.AccountDestination} not exists.");
    }
}