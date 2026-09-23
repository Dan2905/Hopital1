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

    [Fact]
    public void InvoiceRejectsOverpayment()
    {
        var result = HospitalValidation.ValidateInvoice("FAC-2026-0001", 1, 100m, 120m);

        Assert.Equal("Le montant payé ne peut pas dépasser le montant total.", result);
    }

    [Fact]
    public void PaymentRejectsAmountAboveRemaining()
    {
        var result = HospitalValidation.ValidatePayment(100m, 60m, 50m);

        Assert.Equal("Ce paiement dépasserait le montant total de la facture.", result);
    }

    [Fact]
    public void OverlapDetectedWhenAppointmentsIntersect()
    {
        var start = DateTime.Today.AddHours(9);

        Assert.True(HospitalValidation.IsOverlapping(start, TimeSpan.FromMinutes(30), start.AddMinutes(15), TimeSpan.FromMinutes(30)));
        Assert.False(HospitalValidation.IsOverlapping(start, TimeSpan.FromMinutes(30), start.AddMinutes(30), TimeSpan.FromMinutes(30)));
    }
}