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
            track.GeoLocation = new GeoLocation(43.331809, 21.89199);

            track.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            string jsonString = JsonConvert.SerializeObject(track);

            jsonString = jsonString.Replace("Latitude", "lat");
            jsonString = jsonString.Replace("Longitude", "lon");

            var indexResponse = _elasticClient.LowLevel.Index<StringResponse>("timeseries_tracking", PostData.String(jsonString), null);
        }
    }
}
