using System;
using System.Collections.Generic;
using System.Linq;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Exceptions;

public class InvalidAttachmentTypeException : Exception
{
    private readonly List<AttachmentType> types = new();

    public InvalidAttachmentTypeException(params AttachmentType[] acceptableTypes)
    {
        types.AddRange(acceptableTypes);
    }

    public override string Message
    {
        get
        {
            return "Invalid attachment type. Acceptable types: " +
                   types.Aggregate("", (current, next) => current + ", " + next).TrimEnd(',');
        }
    }
}