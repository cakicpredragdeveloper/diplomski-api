using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Infrastructure.Persistance.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly IElasticClient _elasticClient;

        public TrackRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void AddTrack(TrackDto track)
        {
            var indexResponse = _elasticClient.Index<TrackDto>(track, i => i.Index("timeseries_tracking"));
        }

        public void AddTrackList(List<TrackDto> tracks)
        {
            foreach(var track in tracks)
            {
                var indexResponse = _elasticClient.Index<TrackDto>(track, i => i.Index("timeseries_tracking"));
            }
        }

        public List<TrackDto> GetTracksForVehicle(VehicleTrackSearchParameters searchParameters)
        {
            List<TrackDto> result = new List<TrackDto>();

            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = searchParameters.StartDate,
                LessThanOrEqualTo = searchParameters.EndDate
            };

            TermQuery term = new TermQuery { Field = "vin", Value = searchParameters.Vin };

            var queryContainer = new QueryContainer();
            queryContainer &= term;
            queryContainer &= dateRange;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Index("timeseries_tracking"));

            result.AddRange(searchResponse.Documents.ToList<TrackDto>());

            return result;
        }

        public SpeedStatistics GetSpeedStatisticsForVehicle(VehicleTrackSearchParameters searchParameters)
        {
            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = searchParameters.StartDate,
                LessThanOrEqualTo = searchParameters.EndDate
            };

            TermQuery term = new TermQuery { Field = "vin", Value = searchParameters.Vin };

            var queryContainer = new QueryContainer();
            queryContainer &= term;
            queryContainer &= dateRange;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Aggregations(agg => agg.Stats("speed_stats", track => track.Field(field => field.Speed)))
                .Index("timeseries_tracking"));

            var stats = GetSpeedStatisticsFromSearchResponse(searchResponse);

            return stats;
        }

        public double GetKilometrageForVehicle(VehicleTrackSearchParameters searchParameters)
        {
            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = searchParameters.StartDate,
                LessThanOrEqualTo = searchParameters.EndDate
            };

            TermQuery term = new TermQuery { Field = "vin", Value = searchParameters.Vin };

            var queryContainer = new QueryContainer();
            queryContainer &= term;
            queryContainer &= dateRange;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Aggregations(agg => agg.Stats("kilometrage_stats", track => track.Field(field => field.Kilometrage)))
                .Index("timeseries_tracking"));

            var stats = searchResponse.Aggregations.Stats("kilometrage_stats");

            return (double)(stats.Max - stats.Min);
        }

        public KilometrageStatistics GetKilometrageForVehicleByDateInterval(VehicleTrackSearchParameters searchParameter)
        {
            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = searchParameter.StartDate,
                LessThanOrEqualTo = searchParameter.EndDate
            };

            TermQuery term = new TermQuery { Field = "vin", Value = searchParameter.Vin };

            var queryContainer = new QueryContainer();
            queryContainer &= term;
            queryContainer &= dateRange;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Aggregations(agg => agg.DateHistogram("date_histogram", dh => dh.Field(track => track.Timestamp).CalendarInterval(searchParameter.DateInterval)
                        .Aggregations(agg => agg.Stats("kilometrage_stats", track => track.Field(field => field.Kilometrage)))))
                .Index("timeseries_tracking"));

            var result = GetKilometrageStatisticsFromSearchResponse(searchResponse);

            return result;
        }

        public KilometrageStatistics GetKiloMetrageForVehiclesByDateInterval(MarksModelsTrackSearchParameteres searchParameteres)
        {
            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = searchParameteres.StartDate,
                LessThanOrEqualTo = searchParameteres.EndDate
            };

            TermQuery term = new TermQuery { Field = "manufacturerName", Value = searchParameteres.ManufacturerName };

            var queryContainer = new QueryContainer();
            queryContainer &= dateRange;
            queryContainer &= term;

            if(!string.IsNullOrEmpty(searchParameteres.ModelName))
            {
                term = new TermQuery { Field = "modelName", Value = searchParameteres.ModelName };
                queryContainer &= term;
            }

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Aggregations(agg => agg.DateHistogram("date_histogram", dh => dh.Field(track => track.Timestamp).CalendarInterval(searchParameteres.DateInterval)
                        .Aggregations(agg => agg.Stats("kilometrage_stats", track => track.Field(field => field.Kilometrage)))))
                .Index("timeseries_tracking"));

            var result = GetKilometrageStatisticsFromSearchResponse(searchResponse);

            return result;
        }

        public List<TrackDto> GetTracksWithGeoDistanceOfPoint(double lat, double lng, double distance)
        {
            List<TrackDto> result = new List<TrackDto>();

            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = Nest.DateMath.Now.Subtract("3m"),
                LessThanOrEqualTo = Nest.DateMath.Now
            };

            GeoDistanceQuery geoDistanceQuery = new GeoDistanceQuery()
            {
                Field = Infer.Field<TrackDto>(t => t.GeoLocation),
                Distance = new Distance(distance, DistanceUnit.Kilometers),
                Location = new GeoLocation(lat, lng)
            };

            var queryContainer = new QueryContainer();
            queryContainer &= dateRange;
            queryContainer &= geoDistanceQuery;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Index("timeseries_tracking"));

            var vinList = searchResponse.Documents.Select(x => x.Vin).Distinct().ToList();

            foreach(var vin in vinList)
            {
                var track = searchResponse.Documents.Where(doc => doc.Vin == vin).Last();
                result.Add(track);
            }

            return result;
        }

        public List<TrackDto> GetTracksInPolygon(ICollection<Point> points)
        {
            var pointsArray = points.Select(point => new GeoLocation(point.lat, point.lng)).ToArray();

            List<TrackDto> result = new List<TrackDto>();

            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = Nest.DateMath.Now.Subtract("3m"),
                LessThanOrEqualTo = Nest.DateMath.Now
            };

            GeoPolygonQuery geoPolygonQuery = new GeoPolygonQuery()
            {
                Field = Infer.Field<TrackDto>(t => t.GeoLocation),
                Points = points.Select(point => new GeoLocation(point.lat, point.lng)).ToArray()
            };

            var queryContainer = new QueryContainer();
            queryContainer &= dateRange;
            queryContainer &= geoPolygonQuery;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Index("timeseries_tracking"));

            var vinList = searchResponse.Documents.Select(x => x.Vin).Distinct().ToList();

            foreach (var vin in vinList)
            {
                var track = searchResponse.Documents.Where(doc => doc.Vin == vin).Last();
                result.Add(track);
            }

            return result;
        }

        public TrackDto GetCurrentLocationOfVehicle(string vin)
        {
            DateRangeQuery dateRange = new DateRangeQuery()
            {
                Name = "timestamp_range",
                Field = "@timestamp",
                GreaterThanOrEqualTo = Nest.DateMath.Now.Subtract("3m"),
                LessThanOrEqualTo = Nest.DateMath.Now
            };

            TermQuery term = new TermQuery { Field = "vin", Value = vin };

            var queryContainer = new QueryContainer();
            queryContainer &= dateRange;
            queryContainer &= term;

            var searchResponse = _elasticClient.Search<TrackDto>(s => s
                .Query(_ => queryContainer)
                .Sort(sort => sort
                    .Field(fs => fs
                        .Field("@timestamp")
                            .Order(SortOrder.Descending))).Size(1)
                                .Index("timeseries_tracking"));

            if(searchResponse.Documents.Count > 0)
            {
                return searchResponse.Documents.First();
            }

            return null;
        }

        public List<TrackDto> GetVehiclesWithDistance(string vin, double distance)
        {
            var currentPosition = GetCurrentLocationOfVehicle(vin);

            if(currentPosition != null)
            {
                var vehicles = GetTracksWithGeoDistanceOfPoint(currentPosition.GeoLocation.Latitude, currentPosition.GeoLocation.Longitude, distance).Where(x => x.Vin != vin).ToList();
                return vehicles;
            }

            return null;
        }


        #region Helpers

        private KilometrageStatistics GetKilometrageStatisticsFromSearchResponse(ISearchResponse<TrackDto> searchResponse)
        {
            KilometrageStatistics result = new KilometrageStatistics();

            var dateHistogramBuckets = searchResponse.Aggregations.DateHistogram("date_histogram").Buckets;

            foreach (var bucket in dateHistogramBuckets)
            {
                //if(bucket.Stats("kilometrage_stats").Max != null)
                //{
                result.KilometrageByDate.Add(new KilometrageByDate()
                {
                    Date = bucket.Date,
                    Kilometrage = bucket.Stats("kilometrage_stats").Max != null ? (double)(bucket.Stats("kilometrage_stats").Max - bucket.Stats("kilometrage_stats").Min) : 0
                }); 
                //}
            }

            return result;
        }

        private SpeedStatistics GetSpeedStatisticsFromSearchResponse(ISearchResponse<TrackDto> searchResponse)
        {
            SpeedStatistics result = new SpeedStatistics()
            {
                Min = (double)searchResponse.Aggregations.Stats("speed_stats").Min,
                Max = (double)searchResponse.Aggregations.Stats("speed_stats").Max,
                Avg = (double)searchResponse.Aggregations.Stats("speed_stats").Average
            };

            return result;
        }


        #endregion
    }
}
