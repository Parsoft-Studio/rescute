using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using rescute.Application;
using rescute.Application.Reports;
using rescute.Domain.Repositories;
using rescute.Infrastructure;
using rescute.Infrastructure.Repositories;
using rescute.Shared;
using rescute.Web.Configuration;
using rescute.Web.Pages.Reports.ViewModels;

namespace rescute.Web.Extensions;

public static class ApplicationServicesExtensions
{
    /// <summary>
    /// Registers any rescute specific services in the dependency injection container.
    /// </summary>
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopmentEnvironment)
    {
        services.AddOidcAuthentication(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // infrastructure
        // services.AddScoped<IFileStorageService, FileStorageService>();

        // a factory that asks the DI container for a new instance whenever invoked
        services.AddSingleton<Func<IUnitOfWork>>(provider => provider.GetRequiredService<IUnitOfWork>);
        // new unit of work whenever requested
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        // the database context is created for every unit of work
        services.AddTransient<rescuteContext>();

        services.AddSingleton<DbContextOptions<rescuteContext>>(_ =>
            new DbContextOptionsBuilder<rescuteContext>().UseInMemoryDatabase("rescute").Options
        );

        // application
        services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
        services.AddScoped<IReportsService, ReportsService>();

        // UI
        services.AddScoped<ReportsViewModel>();

        if (isDevelopmentEnvironment) // second registration takes precedence
        {
            services.AddTransient<rescuteContext>(provider =>
                new rescuteContext(provider.GetRequiredService<DbContextOptions<rescuteContext>>()).WithFakeData());
        }

        return services;
    }

    private static IServiceCollection AddOidcAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.ClientId = configuration["OIDC:ClientId"];
                options.ClientSecret = configuration["OIDC:ClientSecret"];
                options.Authority = configuration["OIDC:Authority"];
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
            });

        return services;
    }
}