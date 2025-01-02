using Microsoft.EntityFrameworkCore;
using rescute.Application;
using rescute.Application.Reports;
using rescute.Domain.Repositories;
using rescute.Infrastructure;
using rescute.Infrastructure.Repositories;
using rescute.Infrastructure.Services;
using rescute.Shared;
using rescute.Web.Configuration;
using rescute.Web.MockData;
using rescute.Web.Pages.Reports.ViewModels;

namespace rescute.Web.Extensions;

public static class ApplicationServicesExtensions
{
    /// <summary>
    /// Registers any rescute specific services in the dependency injection container.
    /// </summary>
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, bool isDevelopmentEnvironment)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        // infrastructure
        services.AddScoped<ISamaritanRepository, SamaritanRepository>();
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<ITimelineItemRepository, TimelineItemRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        // services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<DbContextOptions<rescuteContext>>(_ =>
            new DbContextOptionsBuilder<rescuteContext>().UseInMemoryDatabase("rescute").Options
        );
        services.AddSingleton<rescuteContext>();

        // application
        services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
        services.AddScoped<IReportsService, ReportsService>();

        // UI
        services.AddScoped<ReportsViewModel>();
        
        if (isDevelopmentEnvironment)   // second registration takes precedence
        {
            services.AddScoped<ITimelineItemRepository, MockTimelineItemRepository>();
        }
        
        return services;
    }
}