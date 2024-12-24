using System;

namespace rescute.Domain.ValueObjects;

public record Attachment
{
    public string FileName { get; }
    public string Extension { get; }
    public AttachmentType Type => GetAttachmentType();
    public string Description { get; }
    public DateTime CreationDate { get; }

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