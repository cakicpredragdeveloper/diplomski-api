using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Dtos
{
    public class TrackDto
    {
        [Keyword]
        public string VehicleId { get; set; }

        [Date]
        public DateTime DateTime { get; set; }

        [GeoPoint]
        public GeoLocation GeoLocation { get; set; }

        [Number]
        public double Speed { get; set; }

        [Number]
        public double Course { get; set; }
    }
}
