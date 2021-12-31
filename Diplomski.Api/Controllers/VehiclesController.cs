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

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public IActionResult GetVehicles([FromQuery] VehiclePaginationParameters filter)
        {
            var vehicles = _vehicleService.GetVehicles(filter);
            return Ok(vehicles);
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
            return Ok(vehicle);
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
