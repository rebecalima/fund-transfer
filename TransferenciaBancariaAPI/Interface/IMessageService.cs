namespace TransferenciaBancariaAPI.Interface
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }
}