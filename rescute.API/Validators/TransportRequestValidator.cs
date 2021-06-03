using FluentValidation;
using rescute.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Validators
{
    public class TransportRequestValidator : AbstractValidator<TransportRequestPostModel>

    {
        public TransportRequestValidator()
        {
            RuleFor(transportRequested => transportRequested.AnimalId)
                .NotNull()
                .NotEmpty()
                .Must(animalId => Guid.TryParse(animalId, out var id));
            RuleFor(transportRequested => transportRequested.Lattitude).InclusiveBetween(-90, 90);
            RuleFor(transportRequested => transportRequested.Longitude).InclusiveBetween(-180, 180);
            RuleFor(transportRequested => transportRequested.ToLattitude).InclusiveBetween(-90, 90);
            RuleFor(transportRequested => transportRequested.ToLongitude).InclusiveBetween(-180, 180);
        }
    }
}
