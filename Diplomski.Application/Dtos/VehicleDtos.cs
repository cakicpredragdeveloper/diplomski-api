using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Dtos
{
    public class VehicleDto
    {
        [Keyword]
        public string Vin { get; set; }

        [Text]
        public string ManufacturerName { get; set; }

        [Text]
        public string ModelName { get; set; }

        [Text]
        public string Transmission { get; set; }

        [Text]
        public string Color { get; set; }

        [Number]
        public int OdometerValue { get; set; }

        [Number]
        public int YearProduced { get; set; }

        [Text]
        public string EngineFuel { get; set; }

        [Boolean]
        public bool EngineHasGas { get; set; }

        [Number]
        public double EngineCapacity { get; set; }

        [Text]
        public string BodyType { get; set; }

        [Text]
        public string Drivetrain { get; set; }

        [Number]
        public double PriceEur { get; set; }

        [Keyword]
        public string MapKey { get; set; }
    }

    public class ManufacturerWithModels
    {
        public string Manufacturer { get; set; }

        public List<string> Models { get; set; }
    }
}
