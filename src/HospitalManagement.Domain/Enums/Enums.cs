namespace HospitalManagement.Domain.Enums;

public enum Sexe
{
    Masculin = 1,
    Feminin = 2,
    Autre = 3
}

public enum GroupeSanguin
{
    Inconnu = 0,
    APositive = 1,
    ANegative = 2,
    BPositive = 3,
    BNegative = 4,
    ABPositive = 5,
    ABNegative = 6,
    OPositive = 7,
    ONegative = 8
}

public enum StatutAdmission
{
    EnCours = 1,
    Sorti = 2,
    Transfere = 3,
    Annulee = 4
}

public enum TypeChambre
{
    Simple = 1,
    Double = 2,
    VIP = 3,
    SoinsIntensifs = 4,
    Pediatric = 5
}

public enum StatutChambre
{
    Libre = 1,
    Occupee = 2,
    EnMaintenance = 3,
    Reservee = 4
}

public enum StatutLit
{
    Libre = 1,
    Occupe = 2,
    EnMaintenance = 3,
    Reserve = 4
}

public enum TypePersonnel
{
    Medecin = 1,
    Infirmier = 2,
    Administration = 3,
    Pharmacien = 4,
    Laborantin = 5,
    Receptionniste = 6,
    Autre = 7
}

public enum StatutRendezVous
{
    Planifie = 1,
    Confirme = 2,
    Annule = 3,
    Termine = 4,
    NoShow = 5
}

public enum StatutConsultation
{
    EnAttente = 1,
    EnCours = 2,
    Terminee = 3,
    Annulee = 4
}

public enum StatutFacture
{
    Emise = 1,
    PartiellementPayee = 2,
    Payee = 3,
    Annulee = 4,
    EnAttente = 5
}

public enum ModePaiement
{
    Especes = 1,
    Carte = 2,
    Virement = 3,
    Cheque = 4,
    MobileMoney = 5,
    Assurance = 6
}

public enum StatutPaiement
{
    EnAttente = 1,
    Confirme = 2,
    Refuse = 3,
    Rembourse = 4
}

public enum StatutCommande
{
    Planifiee = 1,
    EnCours = 2,
    Reçue = 3,
    Annulee = 4
}

public enum StatutMouvementStock
{
    Entree = 1,
    Sortie = 2,
    Peremption = 3,
    Ajustement = 4,
    Retour = 5
}

public enum StatutAnalyse
{
    EnAttente = 1,
    EnCours = 2,
    Terminee = 3,
    Validee = 4,
    Annulee = 5
}

public enum StatutPatient
{
    Actif = 1,
    Inactif = 2,
    Decede = 3,
    Transfere = 4
}

public enum TypeExamen
{
    BilanSanguin = 1,
    Radiologie = 2,
    Imagerie = 3,
    Bacteriologie = 4,
    Biochimie = 5,
    Hormonologie = 6,
    Serologie = 7,
    Autre = 8
}
