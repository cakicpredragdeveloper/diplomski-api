using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces;
using Kafka.Public;
using Kafka.Public.Loggers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diplomski.Application.Services.BackroundServices
{
    public class BackroundTrackReceiver : IHostedService
    {
        private readonly ILogger<BackroundTrackReceiver> _logger;
        private ClusterClient _cluster;
        private ElasticClient _elasticClient;

        public BackroundTrackReceiver(ILogger<BackroundTrackReceiver> logger)
        {
            _logger = logger;
            _elasticClient = new ElasticClient();

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

                var indexResponse = _elasticClient.Index(track, i => i.Index("tracking_3vw1k7aj2fm144974"));
            };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
