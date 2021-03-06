using FluentValidation;
using rescute.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Validators
{
    public class StatusReportedValidator : AbstractValidator<StatusReportedPostModel>

    {
        public StatusReportedValidator()
        {
            RuleFor(statusReported => statusReported.AnimalId)
                .NotNull()
                .NotEmpty()
                .Must(animalId => Guid.TryParse(animalId, out var id));
            RuleFor(statusReported => statusReported.Lattitude).InclusiveBetween(-90, 90);
            RuleFor(statusReported => statusReported.Longitude).InclusiveBetween(-180, 180);
            RuleForEach(statusReported => statusReported.Attachments).ChildRules(attValidator =>attValidator.RuleFor(attachment => attachment.ContentType)
                                                                                                                    .NotEmpty()
                                                                                                                    .Must(contentType => contentType.ToLower().StartsWith("image") || contentType.ToLower().StartsWith("video")));
        }
    }
}
