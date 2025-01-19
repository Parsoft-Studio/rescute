using System;
using System.Collections.Generic;
using System.Linq;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.Exceptions;
using rescute.Domain.ValueObjects;

namespace rescute.Domain.Aggregates;

public class Animal : AggregateRoot<Animal>, IHasAttachments
{
    private readonly List<Attachment> attachments = new();

    public AnimalType Type { get; private set; }
    public DateTime RegistrationDate { get; private set; }
    public string Description { get; private set; }
    public string IdentityCertificateId { get; private set; }
    public Id<Samaritan> IntroducedBy { get; private set; }

    private Animal()
    {
    }

    public Animal(DateTime registrationDate, Id<Samaritan> introducedBy, string description, AnimalType type)
    {
        UpdateRegistrationDate(registrationDate);
        UpdateDescription(description);
        UpdateIntroductionSamaritan(introducedBy);
        UpdateAnimalType(type);
    }

    public IReadOnlyList<Attachment> Attachments =>
        attachments.OrderByDescending(a => a.CreationDate).ToList().AsReadOnly();


    public IReadOnlyList<AttachmentType> AcceptableAttachmentTypes => [AttachmentType.Image()];

    public void AddAttachments(params Attachment[] attachments)
    {
        if (attachments != null)
        {
            if (!Array.TrueForAll(attachments, d => AcceptableAttachmentTypes.Contains(d.Type)))
                throw new InvalidAttachmentTypeException(AcceptableAttachmentTypes.ToArray());

            this.attachments.AddRange(attachments);
        }
    }

    public void ClearAttachments()
    {
        attachments.Clear();
    }

    public void RemoveAttachment(Attachment attachment)
    {
        attachments.Remove(attachment);
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
        IdentityCertificateId = newId;
    }
}