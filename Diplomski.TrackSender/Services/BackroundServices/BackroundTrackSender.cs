using Confluent.Kafka;
using Diplomski.Models.Dtos;
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
        private readonly IProducer<Null, string> _producer;
        private readonly int vehicleNumbers;

        public BackroundTrackSender(ILogger<BackroundTrackSender> logger)
        {
            _logger = logger;

            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();

            vehicleNumbers = 11;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var reader = new StreamReader("C:\\Users\\Predrag\\Desktop\\KAFKA TRACKOVI\\kafka-coordinates.csv"))
            {
                Thread.Sleep(10000);

                while (!reader.EndOfStream)
                {
                    List<TrackDto> signals = new List<TrackDto>();

                    for (int count = 0; count < 11; count++)
                    {
                        if(!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var track = GetTrackFromLine(line);
                            signals.Add(track);
                        }
                    }

                    string json = JsonConvert.SerializeObject(signals);
                    _logger.LogInformation("Sending tracks: \n" + json + "\n\n");

                    await _producer.ProduceAsync("demo-tracks-topic",
                                                new Message<Null, string>() { Value = json },
                                                cancellationToken);

                    Thread.Sleep(60000);
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

            DateTime dateTimeTmp = DateTime.Now;
            string vin = values[0];
            double lat = double.Parse(values[1].Remove(0,1));
            double lng = double.Parse(values[2].Remove(values[2].Count() - 1, 1));
            float speed = float.Parse(values[3]);
            float kilometrageStartOfDay = float.Parse(values[4]);
            float kilometrage = float.Parse(values[5]);
            float fuelLevel = float.Parse(values[6]);
            float totalFuelUsed = float.Parse(values[7]);
            bool started = values[8] == "1" ? true : false;
            string city = values[9];
            float bateryVoltage = float.Parse(values[10]);
            float direction = float.Parse(values[11]);
            string manufacturerName = values[12];
            string modelName = values[13];


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
