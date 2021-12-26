using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using Nest;
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
            var indexResponse = _elasticClient.Index(track, i => i.Index("tracking_" + track.VehicleId.ToLower()));
        }
    }
}
