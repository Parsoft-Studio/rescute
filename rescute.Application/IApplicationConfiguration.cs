using System.Globalization;

namespace rescute.Application;

public interface IApplicationConfiguration
{
    int ReportsPageSize { get; }
    CultureInfo GetDefaultCulture();
    IList<CultureInfo> GetSupportedCultures();
}