namespace FundTransferAPI.Interface
{
    public class TransferBase
    {
        public string AccountDestination { get; set; }
        public string AccountOrigin { get; set; }
        public double Value { get; set; }

    }
}