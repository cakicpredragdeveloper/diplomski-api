using Confluent.Kafka;
using Diplomski.Application.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diplomski.TrackSender.BackroundServices
{
    public class BackroundTrackSender : IHostedService
    {
        private readonly ILogger<BackroundTrackSender> _logger;
        private IProducer<Null, string> _producer;

        public BackroundTrackSender(ILogger<BackroundTrackSender> logger)
        {
            _logger = logger;

            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var reader = new StreamReader("C:\\Users\\Predrag\\Desktop\\Diplomski rad\\Aplikacija\\diplomski-api\\Diplomski.TrackSender\\AVL_DataPoints.csv"))
            {
                Thread.Sleep(10000);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var track = GetTrackFromLine(line);

                    string json = JsonConvert.SerializeObject(track);
                    _logger.LogInformation("Sending track: \n" + json + "\n\n");

                    await _producer.ProduceAsync("demo-tracks-topic",
                                                new Message<Null, string>() { Value = json },
                                                cancellationToken);

                    Thread.Sleep(30000);
                }

                _producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer?.Dispose();
            return Task.CompletedTask;
        }

        #region Helpers

        private TrackDto GetTrackFromLine(string line)
        {
            string[] values = line.Split(',');

            string vin = values[0];
            double lat = Double.Parse(values[1]);
            double lng = Double.Parse(values[2]);
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            float speed = float.Parse(values[4]);
            string place = values[5];
            float kilometrageStartOfDay = float.Parse(values[6]);
            float kilometrage = float.Parse(values[7]);
            float fuelLevel = float.Parse(values[8]);
            string city = values[9];
            float bateryVoltage = float.Parse(values[10]);
            float direction = float.Parse(values[11]);

            return new TrackDto()
            {
                Vin = vin,
                Timestamp = timeStamp,
                GeoLocation = new GeoLocation(lat, lng),
                Speed = speed,
                Place = place,
                KilometrageStartOfDay = kilometrageStartOfDay,
                Kilometrage = kilometrage,
                FuelLevel = fuelLevel,
                City = city,
                BateryVoltage = bateryVoltage,
                Direction = direction
            };
        }

        #endregion
    }
}
