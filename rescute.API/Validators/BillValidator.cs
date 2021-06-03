using FluentValidation;
using rescute.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Validators
{
    public class BillValidator : AbstractValidator<BillPostModel>

    {
        public BillValidator()
        {
            RuleFor(bill => bill.AnimalId)
                .NotNull()
                .NotEmpty()
                .Must(animalId => Guid.TryParse(animalId, out var id));
            RuleFor(bill => bill.Attachments)
                .NotEmpty();
            RuleForEach(bill => bill.Attachments).ChildRules(attValidator => attValidator.RuleFor(attachment => attachment.ContentType)
                                                                                                                    .NotEmpty()
                                                                                                                    .Must(contentType => contentType.ToLower().StartsWith("image") || contentType.ToLower() == ("application/pdf")));
            RuleForEach(bill => bill.RelatedMedicalDocumentIds).ChildRules(idValidator => idValidator.RuleFor(id => id)
                                                                                                              .NotEmpty()
                                                                                                              .Must(id => Guid.TryParse(id, out var docId)));
        }
    }
}
