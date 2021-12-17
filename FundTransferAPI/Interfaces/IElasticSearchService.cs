using Nest;

namespace FundTransferAPI.Interface
{
    public interface IElasticSearchService
    {
        public IElasticClient GetClient();
    }
}