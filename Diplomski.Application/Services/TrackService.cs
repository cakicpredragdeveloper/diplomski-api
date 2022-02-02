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
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;

        public TrackService(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public void AddTrack(TrackDto track)
        {
            _trackRepository.AddTrack(track);
        }

        public void AddTrackList(List<TrackDto> tracks)
        {
            _trackRepository.AddTrackList(tracks);
        }

        public TrackDto GetCurrentLocationOfVehicle(string vin)
        {
            var result = _trackRepository.GetCurrentLocationOfVehicle(vin);

            return result;
        }

        public double GetKilometrageForVehicle(VehicleTrackSearchParameters searchParameters)
        {
            var result = _trackRepository.GetKilometrageForVehicle(searchParameters);

            return result;
        }

        public KilometrageStatistics GetKilometrageForVehicleByDateInterval(VehicleTrackSearchParameters searchParameters)
        {
            var result = _trackRepository.GetKilometrageForVehicleByDateInterval(searchParameters);

            return result;
        }

        public KilometrageStatistics GetKilometrageForVehiclesByDateInterval(MarksModelsTrackSearchParameteres searchParameteres)
        {
            var result = _trackRepository.GetKiloMetrageForVehiclesByDateInterval(searchParameteres);

            return result;
        }

        public SpeedStatistics GetSpeedStatisticsForVehicle(VehicleTrackSearchParameters searchParameters)
        {
            var result = _trackRepository.GetSpeedStatisticsForVehicle(searchParameters);

            return result;
        }

        public List<TrackDto> GetTracksForVehicle(VehicleTrackSearchParameters searchParameters)
        {
            var result = _trackRepository.GetTracksForVehicle(searchParameters);

            return result;
        }

        public List<TrackDto> GetTracksInPolygon(ICollection<Point> points)
        {
            var result = _trackRepository.GetTracksInPolygon(points);
            return result;
        }

        public List<TrackDto> GetTracksWithGeoDistanceOfPoint(double lat, double lng, double distance)
        {
            var result = _trackRepository.GetTracksWithGeoDistanceOfPoint(lat, lng, distance);
            return result;
        }

        public List<TrackDto> GetVehiclesWithDistance(string vin, double distance)
        {
            var result = _trackRepository.GetVehiclesWithDistance(vin, distance);

            return result;
        }
    }
}
