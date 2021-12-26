using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces
{
    public interface IVehicleService
    {
        IList<VehicleDto> GetVehicles(VehiclePaginationParameters paginationParameters);

        IList<VehicleDto> GetAll();

        VehicleDto GetVehicleByVin(string vin);

        void AddVehicle(VehicleDto vehicle);
    }
}