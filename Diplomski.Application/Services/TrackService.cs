using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;

        public TrackService(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public void AddTrack(TrackDto track)
        {
            _trackRepository.AddTrack(track);
        }
    }
}
