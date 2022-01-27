using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Dtos
{
    public abstract class TrackSearchParameters
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public DateInterval DateInterval { get; set; }
    }

    public class VehicleTrackSearchParameters : TrackSearchParameters
    {
        [Required]
        public string Vin { get; set; }
    }

    public class MarksModelsTrackSearchParameteres : TrackSearchParameters
    {
        [Required]
        public string ManufacturerName { get; set; }

        public string ModelName { get; set; }
    }
}
