using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces.ThirdPartyContracts
{
    public interface ITrackRepository
    {
        void AddTrack(TrackDto track);
    }
}
