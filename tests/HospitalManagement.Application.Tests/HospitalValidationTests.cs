using HospitalManagement.Application.Validation;
using Xunit;

namespace HospitalManagement.Application.Tests;

public class HospitalValidationTests
{
    [Fact]
    public void PatientRejectsFutureBirthDate()
    {
        var result = HospitalValidation.ValidatePatient("A", "Patient", DateTime.Today.AddDays(1), null);

        Assert.Equal("La date de naissance ne peut pas être dans le futur.", result);
    }

    [Fact]
    public void PatientRejectsInvalidEmail()
    {
        var result = HospitalValidation.ValidatePatient("A", "Patient", DateTime.Today, "invalid");

        Assert.Equal("L'adresse e-mail n'est pas valide.", result);
    }

    [Fact]
    public void AppointmentRejectsInvalidDuration()
    {
        var result = HospitalValidation.ValidateAppointment(1, 1, DateTime.Now.AddHours(1), 0);

        Assert.Equal("La durée doit être comprise entre 5 minutes et 8 heures.", result);
    }
}