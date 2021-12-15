using Nest;

namespace TransferenciaBancariaAPI.Interface
{
    public interface IElasticSearchService
    {
        public IElasticClient _client { get; }
    }
}