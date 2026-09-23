using System.Net.Mail;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Validation;

public static class HospitalValidation
{
    public static string? ValidatePatient(string? firstName, string? lastName, DateTime birthDate, string? email)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return "Le nom et le prénom sont obligatoires.";
        }

        if (birthDate.Date > DateTime.Today)
        {
            return "La date de naissance ne peut pas être dans le futur.";
        }

        return ValidateEmail(email);
    }

    public static string? ValidateAppointment(int patientId, int personnelId, DateTime date, int durationMinutes)
    {
        if (patientId <= 0 || personnelId <= 0)
        {
            return "Le patient et le professionnel sont obligatoires.";
        }

        if (date < DateTime.Now.AddMinutes(-5))
        {
            return "La date du rendez-vous est déjà passée.";
        }

        return durationMinutes is < 5 or > 480
            ? "La durée doit être comprise entre 5 minutes et 8 heures."
            : null;
    }

    public static string? ValidateInvoice(string? numero, int patientId, decimal total, decimal paye)
    {
        if (string.IsNullOrWhiteSpace(numero))
        {
            return "Le numéro de facture est obligatoire.";
        }

        if (patientId <= 0)
        {
            return "Le patient est obligatoire.";
        }

        if (total < 0)
        {
            return "Le montant total ne peut pas être négatif.";
        }

        if (paye < 0)
        {
            return "Le montant payé ne peut pas être négatif.";
        }

        if (paye > total)
        {
            return "Le montant payé ne peut pas dépasser le montant total.";
        }

        return null;
    }

    public static string? ValidatePayment(decimal invoiceTotal, decimal alreadyPaid, decimal amount)
    {
        if (amount <= 0)
        {
            return "Le montant du paiement doit être supérieur à zéro.";
        }

        if (alreadyPaid + amount > invoiceTotal)
        {
            return "Ce paiement dépasserait le montant total de la facture.";
        }

        return null;
    }

    public static string? ValidatePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return null;
        }

        var digits = new string(phone.Where(char.IsDigit).ToArray());
        return digits.Length is < 6 or > 15
            ? "Le numéro de téléphone n'est pas valide."
            : null;
    }

    public static bool IsOverlapping(DateTime startA, TimeSpan durationA, DateTime startB, TimeSpan durationB)
    {
        var endA = startA.Add(durationA);
        var endB = startB.Add(durationB);
        return startA < endB && startB < endA;
    }

        public static string? ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        try
        {
            _ = new MailAddress(email);
            return null;
        }
        catch (FormatException)
        {
            return "L'adresse e-mail n'est pas valide.";
        }
    }

    /// <summary>
    /// Valide la cohérence d'un séjour hospitalier : patient et lit obligatoires,
    /// dates d'entrée / sortie prévue / sortie réelle cohérentes avec le statut.
    /// </summary>
    public static string? ValidateAdmission(
        int patientId,
        int? litId,
        int? chambreId,
        DateTime dateEntree,
        DateTime? dateSortiePrevue,
        DateTime? dateSortie,
        StatutAdmission statut)
    {
        if (patientId <= 0)
        {
            return "Le patient est obligatoire.";
        }

        if (litId is null or <= 0)
        {
            return "Le lit est obligatoire.";
        }

        if (chambreId is null or <= 0)
        {
            return "La chambre est obligatoire.";
        }

        if (dateEntree == default)
        {
            return "La date d'entrée est obligatoire.";
        }

        if (dateEntree.Date > DateTime.Today)
        {
            return "La date d'entrée ne peut pas être dans le futur.";
        }

        if (dateSortiePrevue.HasValue && dateSortiePrevue.Value.Date < dateEntree.Date)
        {
            return "La date de sortie prévue ne peut pas être antérieure à la date d'entrée.";
        }

        if (dateSortie.HasValue && dateSortie.Value.Date < dateEntree.Date)
        {
            return "La date de sortie ne peut pas être antérieure à la date d'entrée.";
        }

        if (statut == StatutAdmission.EnCours && dateSortie.HasValue)
        {
            return "Un séjour en cours ne peut pas avoir de date de sortie.";
        }

        if (statut == StatutAdmission.Sorti && !dateSortie.HasValue)
        {
            return "Un séjour sorti doit avoir une date de sortie.";
        }

        return null;
    }

    /// <summary>
    /// Détecte le chevauchement de deux séjours sur un même lit.
    /// Un séjour encore ouvert est considéré comme se prolongeant indéfiniment.
    /// </summary>
    public static bool HasOverlappingStay(
        DateTime entreeA,
        DateTime? sortieA,
        DateTime entreeB,
        DateTime? sortieB)
    {
        var finA = sortieA ?? DateTime.MaxValue;
        var finB = sortieB ?? DateTime.MaxValue;
        return entreeA < finB && entreeB < finA;
    }
}
