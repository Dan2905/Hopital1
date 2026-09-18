using Radzen;

namespace HospitalManagement.Web.Components.Dialogs;

/// <summary>
/// Confirmations utilisateur standardisées reposant sur <see cref="DialogService"/>.
/// </summary>
public static class DialogServiceExtensions
{
    private static readonly ConfirmOptions DangerOptions = new()
    {
        OkButtonText = "Supprimer",
        CancelButtonText = "Annuler",
        Width = "460px"
    };

    private static readonly ConfirmOptions NeutralOptions = new()
    {
        OkButtonText = "Confirmer",
        CancelButtonText = "Annuler",
        Width = "460px"
    };

    /// <summary>
    /// Demande confirmation avant une suppression irréversible.
    /// </summary>
    /// <returns><c>true</c> lorsque l'utilisateur confirme la suppression.</returns>
    public static async Task<bool> ConfirmDeleteAsync(
        this DialogService dialog,
        string elementLabel,
        string? precision = null)
    {
        var message = $"Voulez-vous vraiment supprimer {elementLabel} ? Cette action est définitive."
            + (string.IsNullOrWhiteSpace(precision) ? string.Empty : $" {precision}");

        var result = await dialog.Confirm(message, "Confirmation de suppression", DangerOptions);
        return result == true;
    }

    /// <summary>
    /// Demande une confirmation générique (changement de statut, sortie de patient, etc.).
    /// </summary>
    public static async Task<bool> ConfirmActionAsync(
        this DialogService dialog,
        string message,
        string title = "Confirmation")
    {
        var result = await dialog.Confirm(message, title, NeutralOptions);
        return result == true;
    }

    /// <summary>
    /// Affiche une erreur métier issue de la base de données.
    /// </summary>
    public static void NotifyError(this NotificationService notification, string message)
        => notification.Notify(NotificationSeverity.Error, "Action impossible", message);

    /// <summary>
    /// Affiche un succès standardisé.
    /// </summary>
    public static void NotifySuccess(this NotificationService notification, string message)
        => notification.Notify(NotificationSeverity.Success, "Opération réussie", message);

    /// <summary>
    /// Affiche un avertissement standardisé.
    /// </summary>
    public static void NotifyWarning(this NotificationService notification, string message)
        => notification.Notify(NotificationSeverity.Warning, "Attention", message);
}
