using Diplomski.Application.Dtos;
using Diplomski.Application.Interfaces.ThirdPartyContracts;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Infrastructure.Persistance.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IElasticClient _elasticClient;

        public VehicleRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void AddVehicle(VehicleDto vehicle)
        {
            var vehicleWithSameVin = GetVehicleByVin(vehicle.VIN);

            if (vehicleWithSameVin == null)
            {
                var indexResponse = _elasticClient.Index(vehicle, i => i.Index("vehicle"));
            }
        }

        public IList<VehicleDto> GetAll()
        {
            var searchResponse = _elasticClient.Search<VehicleDto>(s => s
                .Query(q => q
                    .MatchAll()
                ).Index("vehicle")
            );

            return searchResponse.Documents.ToList();
        }

        public IList<ManufacturerWithModels> GetManufacturersAndModels()
        {
            List<ManufacturerWithModels> manufacturerWithModels = new List<ManufacturerWithModels>();

            var searchResponse = _elasticClient.Search<VehicleDto>(s =>
                s.Aggregations(aggs =>
                aggs.Terms("by_manufacturer", vehicle => vehicle.Field(x => x.ManufacturerName)
                .Aggregations(a => a.Terms("models", vehicle => vehicle.Field(x => x.ModelName))))).Size(1000).Index("vehicle"));

            var manufacturerGroups = searchResponse.Aggregations.Terms("by_manufacturer").Buckets;

            foreach(var group in manufacturerGroups)
            {
                var models = group.Terms("models").Buckets.Select(b => b.Key).ToList();
                manufacturerWithModels.Add(new ManufacturerWithModels()
                {
                    Manufacturer = group.Key,
                    Models = group.Terms("models").Buckets.Select(b => b.Key).ToList()
                });
            }

            return manufacturerWithModels;
        }

        public VehicleDto GetVehicleByVin(string vin)
        {
            var searchResponse = _elasticClient.Search<VehicleDto>(s => s 
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.VIN)
                        .Query(vin)
                    )
                ).Index("vehicle")
            );

            var document = searchResponse.Documents.FirstOrDefault();

            return document;
        }

        public IList<VehicleDto> GetVehicles(VehiclePaginationParameters paginationParameters)
        {
            //var searchDescriptor = new SearchDescriptor<VehicleDto>();
            //searchDescriptor.Query

            return null;
        }


    }
}
