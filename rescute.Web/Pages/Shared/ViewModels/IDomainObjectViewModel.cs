using Microsoft.Extensions.Localization;
using rescute.Domain;
using rescute.Web.Localization;

namespace rescute.Web.Pages.Shared.ViewModels;

/// <summary>
///     Implemented by any view model that maps to a domain object
/// </summary>
/// <typeparam name="TViewModel">Type of the mapped view model</typeparam>
/// <typeparam name="TDomainObject">Type of the domain object the view model is mapped to</typeparam>
public interface IDomainObjectViewModel<out TViewModel, in TDomainObject>
{
    /// <summary>
    ///     Creates the view model from the provided domain object
    /// </summary>
    /// <param name="domainObject">The domain object to create the view model from</param>
    /// <param name="timeProvider">The <see cref="IDateTimeProvider" /> used to calculate event times, etc.</param>
    /// <param name="localizer">An instance of <see cref="IStringLocalizer{T}"/> used to localize text contained in the view model</param>
    /// <returns>A view model created from the domain object</returns>
    static abstract TViewModel Of(TDomainObject domainObject, IDateTimeProvider timeProvider,
        IStringLocalizer<LocalizationResources> localizer);

    /// <summary>
    ///     Creates a list of view models from the provided list of domain objects
    /// </summary>
    /// <param name="domainObjects">The list of domain objects from which the list of view models is created</param>
    /// <param name="timeProvider">The <see cref="IDateTimeProvider" /> used to calculate event times, etc.</param>
    /// <param name="localizer">An instance of <see cref="IStringLocalizer{T}"/> used to localize text contained in the view model</param>
    /// <returns>List of view models created from the domain objects</returns>
    static abstract IReadOnlyList<TViewModel> Of(IReadOnlyList<TDomainObject> domainObjects,
        IDateTimeProvider timeProvider, IStringLocalizer<LocalizationResources> localizer);
}