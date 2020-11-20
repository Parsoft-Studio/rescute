using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    public abstract class DocumentedLogItem :  ReportLogItem, IDocumented
    {
        private List<Document> documents = new List<Document>();
        public IReadOnlyList<Document> Documents => documents.AsReadOnly();
        public abstract IReadOnlyList<DocumentType> AcceptableDocumentTypes { get; }


        public DocumentedLogItem(DateTime eventDate, rescute.Domain.Aggregates.Samaritan createdBy, string description, params Document[] documents) : base(eventDate, createdBy, description)
        {
            AddDocuments(documents);
        }

        public void AddDocuments(params Document[] documents)
        {
            if (documents != null)
            {
                if (!documents.All(d => this.AcceptableDocumentTypes.Contains(d.Type))) throw new InvalidDocumentType(AcceptableDocumentTypes.ToArray());

                this.documents.AddRange(documents);
            }
        }

        public void ClearDocuments()
        {
            this.documents.Clear();
        }

        public void RemoveDocument(Document document)
        {
            this.documents.Remove(document);
        }
        protected DocumentedLogItem() { }
    }
}
