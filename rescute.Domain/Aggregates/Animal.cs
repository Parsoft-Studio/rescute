using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;
using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rescute.Domain.Aggregates
{
    public class Animal : AggregateRoot<Animal>, IHasAttachments
    {

        private readonly List<Attachment> attachments = new List<Attachment>();

        public IReadOnlyCollection<Attachment> Attachments => attachments.OrderByDescending(a => a.CreationDate).ToList().AsReadOnly();
        public AnimalType Type { get; private set; }


        public IReadOnlyCollection<AttachmentType> AcceptableAttachmentTypes => new AttachmentType[] { AttachmentType.ProfilePicture() };
        public DateTime RegistrationDate { get; private set; }
        public string Description { get; private set; }
        public string BirthCertificateId { get; private set; }
        public Id<Samaritan> IntroducedBy { get; private set; }
        private Animal() { }

        public Animal(DateTime registrationDate, Id<Samaritan> introducedBy, string description, AnimalType type)
        {

            UpdateRegistrationDate(registrationDate);
            UpdateDescription(description);
            UpdateIntroductionSamaritan(introducedBy);
            UpdateAnimalType(type);
        }

        public void UpdateAnimalType(AnimalType type)
        {
            if (type == null) throw new ArgumentException("Value cannot be null.", nameof(type));
            Type = type;
        }

        public void UpdateIntroductionSamaritan(Id<Samaritan> introducedBy)
        {
            if (introducedBy == null) throw new ArgumentException("Value cannot be null.", nameof(introducedBy));
            IntroducedBy = introducedBy;
        }

        public void AddAttachments(params Attachment[] attachments)
        {
            if (attachments != null)
            {
                if (!attachments.All(d => this.AcceptableAttachmentTypes.Contains(d.Type))) throw new InvalidAttachmentType(AcceptableAttachmentTypes.ToArray());

                this.attachments.AddRange(attachments);
            }
        }

        public void ClearAttachments()
        {
            this.attachments.Clear();
        }

        public void RemoveAttachment(Attachment document)
        {
            this.attachments.Remove(document);
        }

        public void UpdateRegistrationDate(DateTime newRegDate)
        {
            RegistrationDate = newRegDate;
        }
        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }
        public void UpdateBirthCertificateId(string newId)
        {
            BirthCertificateId = newId;
        }
        public static Animal RandomTestAnimal(Id<Samaritan> introducedBy)
        {
            return new Animal(DateTime.Now, introducedBy, "test animal", AnimalType.Cat());
        }

    }
}
