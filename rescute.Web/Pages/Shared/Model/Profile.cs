using Microsoft.AspNetCore.Components;

namespace rescute.Web.Pages.Shared.Model;

public class Profile(string image, string displayName)
{
    public string Image { get; } = image;
    public string DisplayName { get; } = displayName;
}