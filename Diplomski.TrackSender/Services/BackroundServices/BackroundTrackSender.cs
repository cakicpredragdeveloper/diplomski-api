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
        private bool first;
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
            using (var reader1 = new StreamReader("C:\\Users\\Predrag\\Desktop\\1G6AH5SX0E0299293.csv"))
            {
                using(var reader2 = new StreamReader("C:\\Users\\Predrag\\Desktop\\WAUKG78E46A840649.csv"))
                {
                    Thread.Sleep(10000);

                    first = true;

                    while (!reader1.EndOfStream && !reader2.EndOfStream)
                    {
                        var line = first == true ? reader1.ReadLine() : reader2.ReadLine();

                        var track = GetTrackFromLine(line);

                        string json = JsonConvert.SerializeObject(track);
                        _logger.LogInformation("Sending track: \n" + json + "\n\n");

                        await _producer.ProduceAsync("demo-tracks-topic",
                                                    new Message<Null, string>() { Value = json },
                                                    cancellationToken);

                        first = !first;

                        Thread.Sleep(60000);
                    }
                    
                    _producer.Flush(TimeSpan.FromSeconds(10));
                }
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
            double lat = double.Parse(values[1]);
            double lng = double.Parse(values[2]);
            DateTime dateTimeTmp = DateTime.Now;
            float totalFuelUsed = float.Parse(values[4]);
            bool started = values[5] == "1" ? true : false;
            float speed = float.Parse(values[6]);
            float kilometrageStartOfDay = float.Parse(values[7]);
            float kilometrage = float.Parse(values[8]);
            float fuelLevel = float.Parse(values[9]);
            string city = values[10];
            float bateryVoltage = float.Parse(values[11]);
            float direction = float.Parse(values[12]);
            string manufacturerName = first == true ? "Kia" : "Kia";
            string modelName = first == true ? "Rio" : "Rio";

            return new TrackDto()
            {
                Vin = vin,
                Timestamp = dateTimeTmp,
                GeoLocation = new GeoLocation(lat, lng),
                Speed = speed,
                Place = String.Empty,
                KilometrageStartOfDay = kilometrageStartOfDay,
                Kilometrage = kilometrage,
                FuelLevel = fuelLevel,
                TotalFuelUsed = totalFuelUsed,
                Started = started,
                City = city,
                BateryVoltage = bateryVoltage,
                Direction = direction,
                ManufacturerName = manufacturerName,
                ModelName = modelName
            };
        }

        #endregion
    }
}
