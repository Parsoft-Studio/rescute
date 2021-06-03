using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public sealed class MedicalDocumentType : ValueObject
    {
        public static MedicalDocumentType LabResults() { return new MedicalDocumentType("Lab Results"); }
        public static MedicalDocumentType Prescription() { return new MedicalDocumentType("Prescription"); }
        public static MedicalDocumentType DoctorsOrders() { return new MedicalDocumentType("Doctor's Orders"); }
        public static MedicalDocumentType IdentityCertificate() { return new MedicalDocumentType("Identity Certificate"); }
        public string Name { get; private set; }
        private MedicalDocumentType(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        private MedicalDocumentType() { }

        public static MedicalDocumentType GetByName(string medicalDocumentType)
        {
            if (medicalDocumentType.ToLower() == LabResults().Name.ToLower()) return LabResults();
            if (medicalDocumentType.ToLower() == Prescription().Name.ToLower()) return Prescription();
            if (medicalDocumentType.ToLower() == DoctorsOrders().Name.ToLower()) return DoctorsOrders();
            if (medicalDocumentType.ToLower() == IdentityCertificate().Name.ToLower()) return IdentityCertificate();
            return null;
        }
    }
}
