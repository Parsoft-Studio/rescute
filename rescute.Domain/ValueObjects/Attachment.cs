using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class Attachment : ValueObject
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
            UpdateExtension(extension);
        }

        private void UpdateExtension(string extension)
        {
            while (extension.StartsWith("."))
            {
                extension = extension[1..];
            }
            Extension = extension.Trim();
        }

        private Attachment() { }


        private AttachmentType GetAttachmentType()
        {
            switch (Extension)
            {
                case "jpg":
                case "png":
                    return AttachmentType.Image();
                case "mp4":
                case "mpg":
                case "avi":
                    return AttachmentType.Video();
                case "pdf":
                    return AttachmentType.Document();
                default:
                    return AttachmentType.Unknown();
            }
        }
    }
}
