using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using UDEMY_PROJECT.Data;
using UDEMY_PROJECT.Models;
using UDEMY_PROJECT.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UDEMY_PROJECTContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("UDEMY_PROJECTContext") ?? throw new InvalidOperationException("Connection string 'UDEMY_PROJECTContext' not found."), new MySqlServerVersion(new Version(8, 0, 39, 9)),
        builder =>
            builder.MigrationsAssembly("UDEMY_PROJECT")
    ));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SeedingService>();
builder.Services.AddTransient<RepositoryService<Seller>>();
builder.Services.AddTransient<RepositoryService<Department>>();
builder.Services.AddTransient<RepositoryService<SalesRecord>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<SeedingService>();
        service.Seed();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(
        new RequestLocalizationOptions() 
        { 
            DefaultRequestCulture = new RequestCulture(CultureInfo.GetCultureInfo("en-US")),
            SupportedCultures = new List<CultureInfo> { CultureInfo.GetCultureInfo("en-US") },
            SupportedUICultures = new List<CultureInfo> { CultureInfo.GetCultureInfo("en-US") }
        }
);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
