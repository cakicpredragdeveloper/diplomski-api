using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Diplomski.Application.Dtos
{
    [ElasticsearchType(RelationName = "track_")]
    public class TrackDto
    {
        [Date(Name = "@timestamp")]
        public DateTime Timestamp { get; set; }

        [Keyword]
        public string Vin { get; set; }

        [GeoPoint]
        public GeoLocation GeoLocation { get; set; }

        [Number]
        public float Speed { get; set; }

        [Keyword]
        public string Place { get; set; }

        [Number]
        public float KilometrageStartOfDay { get; set; }

        [Number]
        public float Kilometrage { get; set; }

        [Number]
        public float FuelLevel { get; set; }

        [Keyword]
        public string City { get; set; }

        [Number]
        public float BateryVoltage { get; set; }

        [Number]
        public float Direction { get; set; }

        [Keyword]
        public string ManufacturerName { get; set; }

        [Keyword]
        public string ModelName { get; set; }
    }

    public class KilometrageByDate
    {
        public DateTime Date { get; set; }

        public double Kilometrage { get; set; }
    }

    public class KilometrageStatistics
    {
        public IList<KilometrageByDate> KilometrageByDate { get; set; } = new List<KilometrageByDate>();
    }

    public class SpeedStatistics
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
    }

    public class Point
    {
        public double lat { get; set; }

        public double lng { get; set; }
    }
}
