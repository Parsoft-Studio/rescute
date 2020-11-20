using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    /// <summary>
    /// Represents an item that has a list of documents attached.
    /// </summary>
    public interface IDocumented
    {
        IReadOnlyList<Document> Documents { get; }
        void AddDocuments(params Document[] documents);
        void ClearDocuments();
        void RemoveDocument(Document document);
        abstract IReadOnlyList<DocumentType> AcceptableDocumentTypes { get; }
    }
}
