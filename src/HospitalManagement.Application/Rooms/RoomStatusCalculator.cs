using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Rooms;

public static class RoomStatusCalculator
{
    public static StatutChambre Calculate(IEnumerable<StatutLit> bedStatuses)
    {
        var statuses = bedStatuses.ToList();

        if (statuses.Count == 0 || statuses.All(status => status == StatutLit.Libre))
        {
            return StatutChambre.Libre;
        }

        if (statuses.Any(status => status == StatutLit.Occupe))
        {
            return StatutChambre.Occupee;
        }

        if (statuses.Any(status => status == StatutLit.Reserve))
        {
            return StatutChambre.Reservee;
        }

        return StatutChambre.EnMaintenance;
    }
}