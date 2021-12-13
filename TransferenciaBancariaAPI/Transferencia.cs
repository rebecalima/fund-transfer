namespace TransferenciaBancariaAPI;

public class Transferencia
{
    public DateTime Date { get; set; }
    public Guid TransactionId { get; private set; }
}
