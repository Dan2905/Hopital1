using HospitalManagement.Application.Validation;
using HospitalManagement.Domain.Enums;
using Xunit;

namespace HospitalManagement.Application.Tests;

public class AdmissionValidationTests
{
    private static readonly DateTime Today = DateTime.Today;

    [Fact]
    public void AdmissionRequiresBed()
    {
        var result = HospitalValidation.ValidateAdmission(
            1, null, 1, Today, null, null, StatutAdmission.EnCours);

        Assert.Equal("Le lit est obligatoire.", result);
    }

    [Fact]
    public void AdmissionRequiresRoom()
    {
        var result = HospitalValidation.ValidateAdmission(
            1, 1, null, Today, null, null, StatutAdmission.EnCours);

        Assert.Equal("La chambre est obligatoire.", result);
    }

    [Fact]
    public void AdmissionAcceptsValidOngoingStay()
    {
        var result = HospitalValidation.ValidateAdmission(
            1, 1, 1, Today, Today.AddDays(3), null, StatutAdmission.EnCours);

        Assert.Null(result);
    }

    [Fact]
    public void AdmissionRejectsPlannedDischargeBeforeEntry()
    {
        var result = HospitalValidation.ValidateAdmission(
            1, 1, 1, Today, Today.AddDays(-1), null, StatutAdmission.EnCours);

        Assert.Equal("La date de sortie prévue ne peut pas être antérieure à la date d'entrée.", result);
    }

    [Fact]
    public void AdmissionRejectsOngoingStayWithDischargeDate()
    {
        var result = HospitalValidation.ValidateAdmission(
            1, 1, 1, Today, null, Today, StatutAdmission.EnCours);

        Assert.Equal("Un séjour en cours ne peut pas avoir de date de sortie.", result);
    }

    [Fact]
    public void AdmissionRequiresDischargeDateWhenClosed()
    {
        var result = HospitalValidation.ValidateAdmission(
            1, 1, 1, Today, null, null, StatutAdmission.Sorti);

        Assert.Equal("Un séjour sorti doit avoir une date de sortie.", result);
    }

    [Fact]
    public void OverlappingStayDetectedOnSameBed()
    {
        var entry = Today.AddDays(-5);
        var discharge = Today;

        Assert.True(HospitalValidation.HasOverlappingStay(entry, discharge, Today.AddDays(-1), null));
        Assert.True(HospitalValidation.HasOverlappingStay(entry, null, Today, Today.AddDays(2)));
        Assert.False(HospitalValidation.HasOverlappingStay(entry, discharge, Today, Today.AddDays(2)));
        Assert.False(HospitalValidation.HasOverlappingStay(entry, discharge, Today.AddDays(-10), entry));
    }
}
