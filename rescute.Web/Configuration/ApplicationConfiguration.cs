using rescute.Application;

namespace rescute.Web.Configuration;

public class ApplicationConfiguration(IConfiguration configuration) : IApplicationConfiguration
{
    private const string ConfigSection = "ApplicationConfiguration";
    private const string ReportsPageSizeConfig = ConfigSection + ":" + "ReportsPageSize";

    public int ReportsPageSize { get; } = configuration.GetValue<int>(ReportsPageSizeConfig);
}