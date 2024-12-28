namespace rescute.Domain.Exceptions;

public class ContributionExceedsRequirementException()
    : InvalidContributionException("Contribution exceeds bill amount.");