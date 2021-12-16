using Nest;
using FundTransferAPI.Interface;

namespace FundTransferAPI.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        public IElasticClient _client { get; }
        public ElasticSearchService()
        {
            var indexName = "transfer";
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex(indexName);
            _client = new ElasticClient(settings);
            _client.Indices.Create(indexName,
            index => index.Map<Transfer>(x => x.AutoMap()));
        }

    }
}