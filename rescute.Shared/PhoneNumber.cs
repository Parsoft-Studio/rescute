using rescute.Shared.Exceptions;

namespace rescute.Shared;

public record PhoneNumber
{
    public static readonly PhoneNumber Empty = new();
    public bool IsMobile { get; private set; }
    public string Value { get; }

    private PhoneNumber()
    {
    }

    public PhoneNumber(bool isMobile, string value)
    {
        Value = value;
        IsMobile = isMobile;
    }

    public override string ToString()
    {
        return Value;
    }
}