using System;

namespace rescute.Domain.ValueObjects;

public record Attachment
{
    public string FileName { get; private set; }
    public string Extension { get; private set; }
    public AttachmentType Type => GetAttachmentType();
    public string Description { get; private set; }
    public DateTime CreationDate { get; private set; }

    public Attachment(string filename, string extension, DateTime creationDate, string description)
    {
        FileName = filename;
        CreationDate = creationDate;
        Description = description;
        Extension = extension.TrimStart('.');
    }

    private Attachment()
    {
    }

    private AttachmentType GetAttachmentType()
    {
        return Extension switch
        {
            "jpg" or "png" => AttachmentType.Image(),
            "mp4" or "mpg" or "avi" => AttachmentType.Video(),
            "pdf" => AttachmentType.Document(),
            _ => AttachmentType.Unknown()
        };
    }
}