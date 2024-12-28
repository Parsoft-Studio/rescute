using rescute.Shared;

namespace rescute.Domain.ValueObjects;

public record AttachmentType
{
    public static  AttachmentType Image() => new("Image");
    public static  AttachmentType Video() => new("Video");
    public static  AttachmentType Document() => new("Document");
    public static  AttachmentType Unknown() => new("Unknown");

    private AttachmentType(string type)
    {
        Type = type;
    }

    private AttachmentType()
    {
    }

    private string Type { get; }

    public override string ToString()
    {
        return Type;
    }
}