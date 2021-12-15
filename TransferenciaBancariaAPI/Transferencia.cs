using TransferenciaBancariaAPI.Interface;

namespace TransferenciaBancariaAPI;

public static class StatusType
{
    public static readonly string INQUEUE = "In Queue";
    public static readonly string PROCESSING = "Processing";
    public static readonly string CONFIRMED = "Confirmed";
    public static readonly string ERROR = "Error";
}
public class Transferencia : TransferenciaData
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string? Error { get; set; }

    public Transferencia() { }
    public Transferencia(TransferenciaData transferencia)
    {
        AccountDestination = transferencia.AccountDestination;
        AccountOrigin = transferencia.AccountOrigin;
        Value = transferencia.Value;
        Date = DateTime.UtcNow.Date;
        Id = Guid.NewGuid();
        Status = StatusType.INQUEUE;
    }
}
