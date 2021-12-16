using FundTransferAPI.Interface;

namespace FundTransferAPI;

public static class StatusType
{
    public static readonly string INQUEUE = "In Queue";
    public static readonly string PROCESSING = "Processing";
    public static readonly string CONFIRMED = "Confirmed";
    public static readonly string ERROR = "Error";
}
public class Transfer : TransferBase
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string? Error { get; set; }

    public Transfer() { }
    public Transfer(TransferBase transfer)
    {
        AccountDestination = transfer.AccountDestination;
        AccountOrigin = transfer.AccountOrigin;
        Value = transfer.Value;
        Date = DateTime.UtcNow.Date;
        Id = Guid.NewGuid();
        Status = StatusType.INQUEUE;
    }
}
