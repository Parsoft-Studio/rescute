using System.Globalization;
using Microsoft.AspNetCore.Localization;
using rescute.Application;
using rescute.Domain;
using rescute.Web.Configuration;

namespace rescute.Web.Localization;

/// <summary>
/// Tracks http calls, extracts locale information contained in them as query string parameters and
/// stores user's locale in a cookie to be picked up and applied by later requests as well.
/// </summary>
public class LocalizationMiddleware(RequestDelegate next, IDateTimeProvider dateTimeProvider)
{
    public const string LocaleKey = "lang";

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Query.ContainsKey(LocaleKey))
        {
            var locale = context.Request.Query[LocaleKey].ToString();
            var requestCulture = new RequestCulture(locale);
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

            context.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookieValue,
                new CookieOptions
                {
                    Expires = dateTimeProvider.UtcNow.AddMonths(1),
                    Secure = context.Request.IsHttps,
                    HttpOnly = false,
                    SameSite = SameSiteMode.Strict
                });
        }

        await next.Invoke(context);
    }

    public static void AddLocalization(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLocalization();
        IApplicationConfiguration appConfig = new ApplicationConfiguration(configuration);

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(appConfig.GetDefaultCulture());
            options.SupportedCultures = appConfig.GetSupportedCultures();
            options.SupportedUICultures = appConfig.GetSupportedCultures();
            options.RequestCultureProviders.OfType<QueryStringRequestCultureProvider>().ToList().ForEach(provider =>
            {
                provider.QueryStringKey = LocaleKey;
                provider.UIQueryStringKey = LocaleKey;
            });
        });
    }
}