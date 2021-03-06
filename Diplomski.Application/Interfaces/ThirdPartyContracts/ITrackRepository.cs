using Diplomski.Models.Dtos;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomski.Application.Interfaces.ThirdPartyContracts
{
    public interface ITrackRepository
    {
        void AddTrack(TrackDto track);

        void AddTrackList(List<TrackDto> tracks);    

        List<TrackDto> GetTracksForVehicle(VehicleTrackSearchParameters searchParameters);

        SpeedStatistics GetSpeedStatisticsForVehicle(VehicleTrackSearchParameters searchParameters);

        double GetKilometrageForVehicle(VehicleTrackSearchParameters searchParameters);

        KilometrageStatistics GetKilometrageForVehicleByDateInterval(VehicleTrackSearchParameters searchParameters);

        KilometrageStatistics GetKiloMetrageForVehiclesByDateInterval(MarksModelsTrackSearchParameteres searchParameteres);

        List<TrackDto> GetTracksWithGeoDistanceOfPoint(double lat, double lng, double distance);

        List<TrackDto> GetTracksInPolygon(ICollection<Point> points);

        TrackDto GetCurrentLocationOfVehicle(string vin);

        List<TrackDto> GetVehiclesWithDistance(string vin, double distance);
    }
}
