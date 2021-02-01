using FluentValidation;
using rescute.API.Models;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rescute.API.Validators
{
    public class AnimalValidator : AbstractValidator<AnimalPostModel>
    {
        public AnimalValidator()
        {
            RuleFor(animal => animal.AnimalType)
                .NotEmpty()
                .Must(animalType => AnimalType.GetByName(animalType) != null);
            //RuleFor(animal => animal.Attachments)
            //    .NotNull()
            //    .Must(attachments => attachments != null && attachments.Any());
        }
    }
}
