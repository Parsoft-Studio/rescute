using System;

namespace rescute.Domain.Exceptions;

public class InvalidContributionException(
    string message = "Invalid contribution. Contribution cannot be null or have an invalid amount.")
    : Exception(message);