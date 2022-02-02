using Diplomski.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces
{
    public interface ITrackService
    {
        void AddTrack(TrackDto track);

        void AddTrackList(List<TrackDto> tracks);

        List<TrackDto> GetTracksForVehicle(VehicleTrackSearchParameters searchParameters);

        SpeedStatistics GetSpeedStatisticsForVehicle(VehicleTrackSearchParameters searchParameters);

        double GetKilometrageForVehicle(VehicleTrackSearchParameters searchParameters);

        KilometrageStatistics GetKilometrageForVehicleByDateInterval(VehicleTrackSearchParameters searchParameters);

        KilometrageStatistics GetKilometrageForVehiclesByDateInterval(MarksModelsTrackSearchParameteres searchParameteres);

        List<TrackDto> GetTracksWithGeoDistanceOfPoint(double lat, double lng, double distance);

        List<TrackDto> GetTracksInPolygon(ICollection<Point> points);

        public TrackDto GetCurrentLocationOfVehicle(string vin);

        public List<TrackDto> GetVehiclesWithDistance(string vin, double distance);
    }
}
