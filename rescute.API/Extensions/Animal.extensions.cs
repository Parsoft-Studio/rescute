using rescute.API.Models;
using rescute.Domain.Aggregates;
using rescute.Domain.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace rescute.API.Extensions
{
    public static class AnimalExtensions
    {
        public static AnimalGetModel ToModel(this Animal animal, string relativeAttachmentsRootPath)
        {
            var model = new AnimalGetModel
            {
                AnimalType = animal.Type.ToString(),
                Description = animal.Description,
                AnimalId = animal.Id.Value,
                IntroducedById = animal.IntroducedBy.Value
            };
            var modelAttachments = new List<AttachmentModel>();
            foreach (Attachment att in animal.Attachments)
            {
                modelAttachments.Add(new AttachmentModel()
                {
                    CreationDate = att.CreationDate,
                    Description = att.Description,
                    Url = Path.Combine(relativeAttachmentsRootPath, att.FileName).Replace("\\", "/"),
                    Type = att.Type.ToString()
                });
            }
            model.Attachments = modelAttachments;

            return model;
        }
        public static IReadOnlyCollection<AnimalGetModel> ToModel(this IEnumerable<Animal> animals, string relativeAttachmentsRootPath)
        {
            var result = new List<AnimalGetModel>();
            foreach (Animal animal in animals)
            {
                result.Add(animal.ToModel(relativeAttachmentsRootPath));
            }

            return result;
        }

    }
}
