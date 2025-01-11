using rescute.Shared;

namespace rescute.Domain.ValueObjects;

public record AttachmentType
{
    public static AttachmentType Image() => new("Image");
    public static AttachmentType Video() => new("Video");
    public static AttachmentType Document() => new("Document");
    public static AttachmentType Unknown() => new("Unknown");

    public string Type { get; private set; }

    private AttachmentType(string type)
    {
        Type = type;
    }

    private AttachmentType()
    {
    }


    public override string ToString()
    {
        return Type;
    }
}