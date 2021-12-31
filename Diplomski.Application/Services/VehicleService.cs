using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public void AddVehicle(VehicleDto vehicle)
        {
            _vehicleRepository.AddVehicle(vehicle);
        }

        public IList<VehicleDto> GetAll()
        {
            var vehicles = _vehicleRepository.GetAll();
            return vehicles;
        }

        public IList<ManufacturerWithModels> GetManufacturersAndModels()
        {
            var result = _vehicleRepository.GetManufacturersAndModels();
            return result;
        }

        public VehicleDto GetVehicleByVin(string vin)
        {
            var vehicle = _vehicleRepository.GetVehicleByVin(vin);
            return vehicle;
        }

        public PaginationResponse<VehicleDto> GetVehicles(VehiclePaginationParameters filter)
        {
            var vehicles = _vehicleRepository.GetVehicles(filter);
            return vehicles;
        }
    }
}
