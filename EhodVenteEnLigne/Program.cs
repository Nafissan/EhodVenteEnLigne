using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models;
using Microsoft.AspNetCore.Identity;
using EhodBoutiqueEnLigne.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EhodBoutiqueEnLigne;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
builder.Services.AddSingleton<ICart, Cart>();
builder.Services.AddSingleton<ILanguageService, LanguageService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
    .AddDataAnnotationsLocalization();

builder.Services.AddDbContext<EhodBDD>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EhodBDD")));

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EhodVLUtilisateurs")));

builder.Services.AddDefaultIdentity<IdentityUser>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

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
    app.SeedDatabase(app.Configuration);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var supportedCultures = new[] { "en-GB", "en-US", "en", "fr-FR", "fr", "es", "wo" }; // Ajouter "wo" pour le wolof
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures.ToArray())
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);


app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

await IdentitySeedData.EnsurePopulated(app);

app.Run();
