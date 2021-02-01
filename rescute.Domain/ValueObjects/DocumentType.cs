using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public sealed class AttachmentType : ValueObject
    {
        public static AttachmentType Image() { return new AttachmentType("Image"); }
        public static AttachmentType Video() { return new AttachmentType("Video"); }
        public static AttachmentType TestResult() { return new AttachmentType("TestResult"); }
        public static AttachmentType Bill() { return new AttachmentType("Bill"); }
        public static AttachmentType ProfilePicture() { return new AttachmentType("ProfilePicture"); }
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
