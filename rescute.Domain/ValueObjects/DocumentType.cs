using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public sealed class AttachmentType : ValueObject
    {
        public static AttachmentType Image = new AttachmentType("Image");
        public static AttachmentType Video = new AttachmentType("Video");
        public static AttachmentType TestResult = new AttachmentType("TestResult");
        public static AttachmentType Bill = new AttachmentType("Bill");
        public static AttachmentType ProfilePicture = new AttachmentType("ProfilePicture");
        public string Type { get; private set; }
        public AttachmentType(string type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return this.Type;
        }
        private AttachmentType() { }
    }
}
