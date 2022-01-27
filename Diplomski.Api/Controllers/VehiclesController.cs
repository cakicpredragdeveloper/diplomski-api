using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diplomski.Api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ITrackService _trackService;

        public VehiclesController(IVehicleService vehicleService, ITrackService trackService)
        {
            _vehicleService = vehicleService;
            _trackService = trackService;
        }

        [HttpGet]
        public IActionResult GetVehicles([FromQuery] VehiclePaginationParameters filter)
        {
            var paginationResponse = _vehicleService.GetVehicles(filter);
            return Ok(paginationResponse);
        }

        [HttpGet("index")]
        public IActionResult Get()
        {
            return Ok("hello world");
        }

        [HttpGet]
        [Route("get-all")]
        public IActionResult GetAll()
        {
            var vehicles = _vehicleService.GetAll();
            return Ok(vehicles);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult GetVehicleById([FromQuery] string id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("get-by-vin")]
        public IActionResult GetVehicleByVin([FromQuery] string vin)
        {
            var vehicle = _vehicleService.GetVehicleByVin(vin);
            return Ok(vehicle);
        }

        [HttpPost]
        [Route("add-vehicle")]
        public IActionResult AddVehicle([FromBody] VehicleDto vehicle)
        {
            _vehicleService.AddVehicle(vehicle);
            _vehicleService.AddTrackingIndexForVehicle(vehicle);

            return Ok(vehicle);
        }

        [HttpPost]
        [Route("add-tracking-index-for-vehicle")]
        public IActionResult AddTrackingIndexForVehicle(string vehicleVin)
        {
            var vehicle = _vehicleService.GetVehicleByVin(vehicleVin);

            if (vehicle != null)
            {
                _vehicleService.AddTrackingIndexForVehicle(vehicle);
            }

            return Ok();
        }

        [HttpGet]
        [Route("get-manufacturers-and-models")]
        public IActionResult GetManufacturersAndModels()
        {
            var result = _vehicleService.GetManufacturersAndModels();
            return Ok(result);
        }
    }
}
