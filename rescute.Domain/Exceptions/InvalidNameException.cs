using System;

namespace rescute.Domain.Exceptions;

public abstract class InvalidNameException(string message) : Exception(message);