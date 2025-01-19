using rescute.Domain.Exceptions;

namespace rescute.Domain.ValueObjects;

public record Name
{
    public static Name Empty => new(string.Empty);
    public string Value { get; }
    public int MaxLength { get; private set; }

    public Name(string value)
    {
        Value = value;
    }

    public Name(string value, int maxLength)
    {
        if (value.Length > maxLength) throw new NameTooLongException();
        Value = value;
        MaxLength = maxLength;
    }

    public Name(string value, int maxLength, bool canBeEmpty)
    {
        if (!canBeEmpty && string.IsNullOrWhiteSpace(value)) throw new NameEmptyException();
        if (value.Length > maxLength) throw new NameTooLongException();
        Value = value;
        MaxLength = maxLength;
    }

    private Name()
    {
    }

    public bool IsNameEmpty()
    {
        return string.IsNullOrWhiteSpace(Value);
    }

    public override string ToString()
    {
        return Value;
    }
}