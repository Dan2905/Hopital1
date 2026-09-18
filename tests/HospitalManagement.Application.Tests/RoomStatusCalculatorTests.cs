using HospitalManagement.Application.Rooms;
using HospitalManagement.Domain.Enums;
using Xunit;

namespace HospitalManagement.Application.Tests;

public class RoomStatusCalculatorTests
{
    [Fact]
    public void EmptyRoomIsAvailable()
    {
        Assert.Equal(StatutChambre.Libre, RoomStatusCalculator.Calculate([]));
    }

    [Fact]
    public void OccupiedBedTakesPriority()
    {
        var result = RoomStatusCalculator.Calculate([StatutLit.Reserve, StatutLit.Occupe]);

        Assert.Equal(StatutChambre.Occupee, result);
    }

    [Fact]
    public void MaintenanceIsUsedWhenNoBedIsAvailable()
    {
        var result = RoomStatusCalculator.Calculate([StatutLit.EnMaintenance, StatutLit.EnMaintenance]);

        Assert.Equal(StatutChambre.EnMaintenance, result);
    }
}