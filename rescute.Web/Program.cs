using Microsoft.Extensions.Options;
using rescute.Web;
using rescute.Web.Extensions;
using rescute.Web.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();
builder.Services
    .RegisterApplicationServices(builder.Configuration, builder.Environment.IsDevelopment());

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseMiddleware<LocalizationMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
    {
        Console.WriteLine($@"Unhandled exception: {e.ExceptionObject}");
    };
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();
await app.RunAsync();