using System.ComponentModel;

namespace rescute.Domain.ValueObjects;

public record MedicalDocumentType
{
    public string Name { get; }

    public static MedicalDocumentType LabResults() => new("Lab Results");

    public static MedicalDocumentType Prescription() => new("Prescription");

    public static MedicalDocumentType DoctorsOrders() => new("Doctor's Orders");

    public static MedicalDocumentType IdentityCertificate() => new("Identity Certificate");

    private MedicalDocumentType(string name)
    {
        Name = name;
    }

    private MedicalDocumentType()
    {
    }

    public override string ToString()
    {
        return Name;
    }

    public static MedicalDocumentType GetByName(string medicalDocumentType)
    {
        if (medicalDocumentType.Equals(LabResults().Name)) return LabResults();
        if (medicalDocumentType.Equals(Prescription().Name)) return Prescription();
        if (medicalDocumentType.Equals(DoctorsOrders().Name)) return DoctorsOrders();
        if (medicalDocumentType.Equals(IdentityCertificate().Name)) return IdentityCertificate();

        throw new InvalidEnumArgumentException(nameof(medicalDocumentType));
    }
}