using Nest;
using TransferenciaBancariaAPI.Interface;

namespace TransferenciaBancariaAPI.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        public IElasticClient _client { get; }
        public ElasticSearchService()
        {
            var indexName = "transferencia";
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex(indexName);
            _client = new ElasticClient(settings);
            _client.Indices.Create(indexName,
            index => index.Map<Transferencia>(x => x.AutoMap()));
        }

    }
}