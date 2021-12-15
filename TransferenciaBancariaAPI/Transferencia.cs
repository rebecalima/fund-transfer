namespace TransferenciaBancariaAPI;

public static class Status
{
    public static readonly string INQUEUE = "In Queue";
    public static readonly string PROCESSING = "Processing";
    public static readonly string CONFIRMED = "Confirmed";
    public static readonly string ERROR = "Error";
}
public class Transferencia
{
    public Guid Id { get; set; }
    public string AccountDestination { get; set; }
    public string AccountOrigin { get; set; }
    public double Value { get; set; }
    public DateTime Date { get; }
    public string StatusMessage { get; set; }

    public Transferencia()
    {
        Date = DateTime.UtcNow.Date;
        Id = Guid.NewGuid();
        StatusMessage = Status.INQUEUE;
    }
    public Transferencia(DateTime date, Guid transactionId, string status)
    {
        Date = date;
        Id = transactionId;
        StatusMessage = status;
    }
}
