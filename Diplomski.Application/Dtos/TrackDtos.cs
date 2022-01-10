using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Dtos
{
    public class TrackDto
    {
        [JsonProperty("@timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("vin")]
        [Keyword]
        public string Vin { get; set; }

        [JsonProperty("geoLocation")]
        [GeoPoint]
        public GeoLocation GeoLocation { get; set; }

        [JsonProperty("speed")]
        [Number]
        public float Speed { get; set; }

        [JsonProperty("place")]
        [Keyword]
        public string Place { get; set; }

        [JsonProperty("kilometrageStartOfDay")]
        public float KilometrageStartOfDay { get; set; }

        [JsonProperty("kilometrage")]
        public float Kilometrage { get; set; }

        [JsonProperty("fuelLevel")]
        public float FuelLevel { get; set; }

        [JsonProperty("City")]
        [Keyword]
        public string City { get; set; }

        [JsonProperty("bateryVoltage")]
        public float BateryVoltage { get; set; }

        [JsonProperty("direction")]
        public float Direction { get; set; }
    }
}
