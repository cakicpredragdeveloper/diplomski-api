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

        public YearProducedFilter YearProduced { get; set; }

        public OdometerValueFilter OdometerValue { get; set; }

        public string EngineFuel { get; set; }

        public bool EngineHasGas { get; set; }

        public string Drivetrain { get; set; }
    }

    public class YearProducedFilter
    {
        public int? From { get; set; }

        public int? To { get; set; }
    }

    public class OdometerValueFilter
    {
        public int? From { get; set; }

        public int? To { get; set; }
    }
}
