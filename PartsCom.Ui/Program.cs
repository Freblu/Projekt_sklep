using PartsCom.Application;
using PartsCom.Infrastructure;
using PartsCom.Infrastructure.Extensions;
using PartsCom.Ui.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry().AddDefaultHealthChecks();

builder
    .Services.AddServiceDiscovery()
    .ConfigureHttpClientDefaults(http =>
    {
        http.AddStandardResilienceHandler();
        http.AddServiceDiscovery();
    });

builder.Services.InstallApplication().InstallInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

// Configure cookie policy for secure authentication
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

WebApplication app = builder.Build();

await app.Services.ApplyMigrationsAndSeedAsync();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseStaticFiles();
app.UseRouting();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();
