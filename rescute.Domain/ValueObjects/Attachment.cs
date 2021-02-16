﻿using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class Attachment : ValueObject
    {
        public string FileName { get; private set; }
        public AttachmentType Type { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Attachment(AttachmentType type, string filename, DateTime creationDate, string description)
        {
            FileName = filename;
            Type = type;
            CreationDate = creationDate;
            Description = description;
            
        }
        private Attachment() { }

        public static object Should()
        {
            throw new NotImplementedException();
        }
    }
}