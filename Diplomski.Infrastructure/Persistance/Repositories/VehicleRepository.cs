﻿using Diplomski.Application.Dtos;
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
                aggs.Terms("by_manufacturer", vehicle => vehicle.Field(x => x.ManufacturerName).Size(1000)
                .Aggregations(a => a.Terms("models", vehicle => vehicle.Field(x => x.ModelName).Size(1000))))).Index("vehicle"));

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

        public IList<VehicleDto> GetVehicles(VehiclePaginationParameters filter)
        {
            var queryContainer = new QueryContainer();

            #region Prepare queries

            TermQuery query = null;
            NumericRangeQuery rangeQuery = null; 
            
            if(!string.IsNullOrEmpty(filter.ManufacturerName))
            {
                query = new TermQuery { Field = "manufacturerName", Value = filter.ManufacturerName };
                queryContainer &= query;

                if(!string.IsNullOrEmpty(filter.ModelName))
                {
                    query = new TermQuery { Field = "modelName", Value = filter.ModelName };
                    queryContainer &= query;
                }
            }

            if(filter.YearProduced != null)
            {
                rangeQuery = new NumericRangeQuery()
                {
                    Name = "yearProduced_range",
                    Field = "yearProduced",
                    GreaterThanOrEqualTo = filter.YearProduced.From ?? null,
                    LessThanOrEqualTo = filter.YearProduced.To ?? null
                };

                queryContainer &= rangeQuery;
            }

            if(filter.OdometerValue != null)
            {
                rangeQuery = new NumericRangeQuery()
                {
                    Name = "odometerValue_range",
                    Field = "odometerValue",
                    GreaterThanOrEqualTo = filter.OdometerValue.From ?? null,
                    LessThanOrEqualTo = filter.OdometerValue.To ?? null
                };

                queryContainer &= rangeQuery;
            }

            if (!string.IsNullOrEmpty(filter.EngineFuel))
            {
                query = new TermQuery { Field = "engineFuel", Value = filter.EngineFuel };
                queryContainer &= query;

                query = new TermQuery { Field = "engineHasGas", Value = filter.EngineHasGas };
                queryContainer &= query;
            }

            if(!string.IsNullOrEmpty(filter.Drivetrain))
            {
                query = new TermQuery { Field = "driveTrain", Value = filter.Drivetrain };
                queryContainer &= query;
            }

            #endregion

            var searchResponse = _elasticClient.Search<VehicleDto>(s => s
            .Query(_ => queryContainer)
            .From(filter.PageIndex * filter.PageSize)
            .Size(filter.PageSize)
            .Index("vehicle"));

            return searchResponse.Documents.ToList();
        }
    }
}
