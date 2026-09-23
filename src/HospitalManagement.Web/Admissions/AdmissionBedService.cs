using HospitalManagement.Application.Rooms;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Web.Admissions;

/// <summary>
/// Gère l'occupation et la libération des lits lors des mouvements de séjour,
/// puis recalcule le statut de la chambre concernée.
/// </summary>
public sealed class AdmissionBedService
{
    private readonly ApplicationDbContext db;

    public AdmissionBedService(ApplicationDbContext db) => this.db = db;

    /// <summary>
    /// Marque le lit comme occupé et rafraîchit l'état de sa chambre.
    /// </summary>
    public async Task OccupyAsync(int? litId, CancellationToken cancellationToken = default)
    {
        var bed = await FindBedAsync(litId, cancellationToken);
        if (bed is null)
        {
            return;
        }

        bed.Statut = StatutLit.Occupe;
        await RefreshRoomStatusAsync(bed.ChambreId, cancellationToken);
    }

    /// <summary>
    /// Libère le lit (sauf s'il est en maintenance) et rafraîchit l'état de sa chambre.
    /// </summary>
    public async Task ReleaseAsync(int? litId, CancellationToken cancellationToken = default)
    {
        var bed = await FindBedAsync(litId, cancellationToken);
        if (bed is null)
        {
            return;
        }

        if (bed.Statut != StatutLit.EnMaintenance)
        {
            bed.Statut = StatutLit.Libre;
        }

        await RefreshRoomStatusAsync(bed.ChambreId, cancellationToken);
    }

    /// <summary>
    /// Recalcule le statut d'une chambre à partir de l'état de ses lits.
    /// Une chambre en maintenance conserve son statut.
    /// </summary>
    public async Task RefreshRoomStatusAsync(int chambreId, CancellationToken cancellationToken = default)
    {
        var room = await db.Chambres
            .Include(chambre => chambre.Lits)
            .FirstOrDefaultAsync(chambre => chambre.Id == chambreId, cancellationToken);

        if (room is null || room.Statut == StatutChambre.EnMaintenance)
        {
            return;
        }

        room.Statut = RoomStatusCalculator.Calculate(room.Lits.Select(bed => bed.Statut));
    }

    private Task<Lit?> FindBedAsync(int? litId, CancellationToken cancellationToken) =>
        litId is null or <= 0
            ? Task.FromResult<Lit?>(null)
            : db.Lits.FirstOrDefaultAsync(bed => bed.Id == litId.Value, cancellationToken);
}
