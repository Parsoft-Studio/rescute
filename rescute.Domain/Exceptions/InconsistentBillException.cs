namespace rescute.Domain.Exceptions;

public class InconsistentBillException() : InvalidContributionException(
    "The contributing Samaritan doesn't confirm this Bill's consistency: " +
    "Bill's owner claimed they have attached MedicalDocuments the Bill is paying for, " +
    "but the contribution Samaritan doesn't concur that all claimed documents are attached.");