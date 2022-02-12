using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Diplomski.Api.Controllers
{
    [ApiController]
    public class TrackingController : Controller
    {
        private readonly ITrackService _trackService;

        public TrackingController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpPost("create-track")]
        public IActionResult CreateTrack(TrackDto track)
        {
            _trackService.AddTrack(track);
            return Ok();
        }

        [HttpGet("import-from-csv")]
        public IActionResult ImportFromCsv()
        {
            List<TrackDto> tracks = new List<TrackDto>();
            using (var reader = new StreamReader("C:\\Users\\Predrag\\Desktop\\1202_vehicles.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var track = GetTrackFromLine(line);
                    tracks.Add(track);
                }
            }

            _trackService.AddTrackList(tracks);

            return Ok();
        }

        [HttpGet("get-tracks")]
        public IActionResult GetTracksForVehicle([FromQuery]VehicleTrackSearchParameters searchParameters)
        {
            var tracks = _trackService.GetTracksForVehicle(searchParameters);
            return Ok(tracks);
        }

        [HttpGet("get-speed-statistics")]
        public IActionResult GetSpeedStatisticsForVehicle([FromQuery] VehicleTrackSearchParameters searchParameters)
        {
            var speedStatistics =_trackService.GetSpeedStatisticsForVehicle(searchParameters);
            return Ok(speedStatistics);
        }

        [HttpGet("get-kilometrage")]
        public IActionResult GetKilometrageForVehicle([FromQuery] VehicleTrackSearchParameters searchParameters)
        {
            var kilometrage = _trackService.GetKilometrageForVehicle(searchParameters);
            return Ok(kilometrage);
        }

        [HttpGet("get-kilometrage-by-date-interval")]
        public IActionResult GetKilometrageForVehicleByDateInterval([FromQuery] VehicleTrackSearchParameters searchParameters)
        {
            var kilometrageStatistics = _trackService.GetKilometrageForVehicleByDateInterval(searchParameters);
            return Ok(kilometrageStatistics);
        }

        [HttpGet("get-kilometrage-by-manufacturer-and-date-interval")]
        public IActionResult GetKilometrageForVehiclesByDateInterval([FromQuery] MarksModelsTrackSearchParameteres searchParameteres)
        {
            var kilometrageStatistics = _trackService.GetKilometrageForVehiclesByDateInterval(searchParameteres);
            return Ok(kilometrageStatistics);
        }

        [HttpGet("vehicles-with-geo-distance-from-point")]
        public IActionResult GetTracksWithGeoDistanceOfPoint(double lat, double lng, double distance)
        {
            var vehicles =_trackService.GetTracksWithGeoDistanceOfPoint(lat, lng, distance);

            return Ok(vehicles);
        }

        [HttpPost("vehicles-within-polygon")]
        public IActionResult GetTracksInPolygon([FromBody]ICollection<Point> points)
        {
            var vehicles = _trackService.GetTracksInPolygon(points);

            return Ok(vehicles);
        }

        [HttpGet("current-position")]
        public IActionResult GetCurrentLocationOfVehicle(string vin)
        {
            var track = _trackService.GetCurrentLocationOfVehicle(vin);

            return Ok(track);
        }

        [HttpGet("vehicles-with-geo-distance-from-vehicle")]
        public IActionResult GetVehiclesWithDistance(string vin, double distance)
        {
            var vehicles = _trackService.GetVehiclesWithDistance(vin, distance);

            return Ok(vehicles);
        }

        #region Helpers

        private TrackDto GetTrackFromLine(string line)
        {
            string[] values = line.Split(',');

            string vin = values[0];
            double lat = double.Parse(values[1]);
            double lng = double.Parse(values[2]);
            DateTime dateTimeTmp = DateTime.Parse(values[3]);
            float totalFuelUsed = float.Parse(values[4]);
            bool started = values[5] == "1" ? true : false;
            float speed = float.Parse(values[6]);
            float kilometrageStartOfDay = float.Parse(values[7]);
            float kilometrage = float.Parse(values[8]);
            float fuelLevel = float.Parse(values[9]);
            string city = values[10];
            float bateryVoltage = float.Parse(values[11]);
            float direction = float.Parse(values[12]);
            string manufacturerName = values[13];
            string modelName = values[14];

            return new TrackDto()
            {
                Vin = vin,
                Timestamp = dateTimeTmp,
                GeoLocation = new GeoLocation(lat, lng),
                Speed = speed,
                Place = String.Empty,
                KilometrageStartOfDay = kilometrageStartOfDay,
                Kilometrage = kilometrage,
                FuelLevel = fuelLevel,
                TotalFuelUsed = totalFuelUsed,
                Started = started,
                City = city,
                BateryVoltage = bateryVoltage,
                Direction = direction,
                ManufacturerName = manufacturerName,
                ModelName = modelName
            };
        }

        #endregion
    }
}
