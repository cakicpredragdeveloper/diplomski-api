using Diplomski.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces
{
    public interface IVehicleService
    {
        PaginationResponse<VehicleDto> GetVehicles(VehiclePaginationParameters filter);

        IList<VehicleDto> GetAll();

        VehicleDto GetVehicleByVin(string vin);

        void AddVehicle(VehicleDto vehicle);

        IList<ManufacturerWithModels> GetManufacturersAndModels();

        void AddTrackingIndexForVehicle(VehicleDto vehicle);
    }
}