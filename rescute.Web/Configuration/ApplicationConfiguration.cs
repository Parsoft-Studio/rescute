using System.Globalization;
using rescute.Application;

namespace rescute.Web.Configuration;

public class ApplicationConfiguration(IConfiguration configuration) : IApplicationConfiguration
{
    private const string ConfigSection = "ApplicationConfiguration";
    private const string ReportsPageSizeConfig = ConfigSection + ":" + "ReportsPageSize";
    private const string DefaultCultureConfig = ConfigSection + ":" + "DefaultCulture";
    private const string SupportedCulturesConfig = ConfigSection + ":" + "SupportedCultures";

    public int ReportsPageSize { get; } = configuration.GetValue<int>(ReportsPageSizeConfig);

    public CultureInfo GetDefaultCulture() => GetSupportedCultures()
        .Single(cultureInfo => cultureInfo.Name == configuration.GetValue<string>(DefaultCultureConfig));

    public IList<CultureInfo> GetSupportedCultures() =>
        configuration.GetSection(SupportedCulturesConfig).Get<List<string>>()!
            .Select(CultureInfo.GetCultureInfo).ToList();
}