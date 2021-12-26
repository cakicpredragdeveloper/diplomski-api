using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces
{
    public interface ITrackService
    {
        void AddTrack(TrackDto track);
    }
}
