namespace rescute.Web.Pages.Shared.ViewModels;

public class Profile(string image, string displayName)
{
    public string Image { get; } = image;
    public string DisplayName { get; } = displayName;
}