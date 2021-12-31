using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces.ThirdPartyContracts
{
    public interface IVehicleRepository
    {
        IList<VehicleDto> GetVehicles(VehiclePaginationParameters filter);

        IList<VehicleDto> GetAll();

        VehicleDto GetVehicleByVin(string vin);

        void AddVehicle(VehicleDto vehicle);

        IList<ManufacturerWithModels> GetManufacturersAndModels();
    }
}
