using System;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Exceptions;

public class MissingMedicalDocumentTypeException(MedicalDocumentType missingType)
    : Exception($"The following medical document type expected but missing: {missingType}");