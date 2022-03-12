using Diplomski.Models.Dtos;
using Diplomski.Application.Interfaces;
using Elasticsearch.Net;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diplomski.Application.Services.BackroundServices
{
    public class BackroundTrackReceiver : IHostedService
    {
        private readonly ILogger<BackroundTrackReceiver> _logger;
        private readonly ElasticSearchOptions _options;
        private ClusterClient _cluster;
        private ElasticClient _elasticClient;

        public BackroundTrackReceiver(ILogger<BackroundTrackReceiver> logger, ElasticSearchOptions options)
        {
            _logger = logger;
            _options = options;

            var settings = new ConnectionSettings(new Uri(_options.Url));
            settings.BasicAuthentication(_options.Username, _options.Password);
            _elasticClient = new ElasticClient(settings);

            _cluster = new ClusterClient(new Kafka.Public.Configuration
            {
                Seeds = "localhost:9092"
            }, new ConsoleLogger());
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cluster.ConsumeFromLatest("demo-tracks-topic");
            _cluster.MessageReceived += record =>
            {
                _logger.LogInformation($"Received: {Encoding.UTF8.GetString(record.Value as byte[])}\n");

                string json = Encoding.UTF8.GetString(record.Value as byte[]);

                TrackDto track = JsonConvert.DeserializeObject<TrackDto>(json);

                var indexResponse = _elasticClient.Index<TrackDto>(track, i => i.Index("timeseries_tracking"));
            };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
