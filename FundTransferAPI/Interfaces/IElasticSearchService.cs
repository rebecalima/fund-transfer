using Nest;

namespace FundTransferAPI.Interface
{
    public interface IElasticSearchService
    {
        public IElasticClient _client { get; }
    }
}