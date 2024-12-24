using System;

namespace rescute.Shared.Exceptions;

public abstract class InvalidNameException(string message) : Exception(message);