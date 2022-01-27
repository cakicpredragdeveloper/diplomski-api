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
            using (var reader = new StreamReader("C:\\Users\\Predrag\\Desktop\\vehicles_tracking_csv.csv"))
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

        [HttpGet("vehicles-in-rectangle")]
        public IActionResult GetTracksInRectangle(double ltLat, double ltLng, double rbLat, double rbLng)
        {
            var vehicles = _trackService.GetTracksInRectangle(ltLat, ltLng, rbLat, rbLng);

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
            double lat = Double.Parse(values[1]);
            double lng = Double.Parse(values[2]);
            DateTime dateTimeTmp = DateTime.Parse(values[3]);
            float speed = float.Parse(values[4]);
            float kilometrageStartOfDay = float.Parse(values[5]);
            float kilometrage = float.Parse(values[6]);
            float fuelLevel = float.Parse(values[7]);
            string city = values[8];
            float bateryVoltage = float.Parse(values[9]);
            float direction = float.Parse(values[10]);
            string manufacturerName = values[11];
            string modelName = values[12];

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
