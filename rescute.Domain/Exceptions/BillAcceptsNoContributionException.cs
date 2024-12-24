namespace rescute.Domain.Exceptions;

internal class BillAcceptsNoContributionException()
    : InvalidContributionException("The bill doesn't accept contributions.");