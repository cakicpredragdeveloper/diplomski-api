using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Dtos
{
    public class VehiclePaginationParameters
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public string ManufacturerName { get; set; }

        public string ModelName { get; set; }

        public string EngineFuel { get; set; }

        public bool EngineHasGas { get; set; }

        public string Drivetrain { get; set; }

        public int? YearFrom { get; set; }

        public int? YearTo { get; set; }

        public int? OdometerFrom { get; set; }

        public int? OdometerTo { get; set; }
    }
}
