namespace HospitalManagement.Web.Components.Dialogs;

public sealed class EditUserRolesDialogArgs
{
    public string UserId { get; }
    public List<string> CurrentRoles { get; }

    public EditUserRolesDialogArgs(string userId, List<string> currentRoles)
    {
        UserId = userId;
        CurrentRoles = currentRoles;
    }
}
