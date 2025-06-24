using FluentValidation.AspNetCore;
using ISUAnket.Business.Interfaces;
using ISUAnket.Business.Managers;
using ISUAnket.DataAccess.Context;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.DataAccess.Repositories;
using ISUAnket.DataAccess.Seed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuestPDF.Infrastructure;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//builder.Services.AddDbContext<ISUAnketContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ISUAnketContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsHistoryTable("__EFMigrationsHistory", "ANKET")));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));

builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IKullaniciService, KullaniciManager>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolManager>();

builder.Services.AddScoped<IAnketRepository, AnketRepository>();
builder.Services.AddScoped<IAnketService, AnketManager>();

builder.Services.AddScoped<ISoruRepository, SoruRepository>();
builder.Services.AddScoped<ISoruService, SoruManager>();

builder.Services.AddScoped<ICevapRepository, CevapRepository>();
builder.Services.AddScoped<ICevapService, CevapManager>();

builder.Services.AddScoped<IBirimRepository, BirimRepository>();
builder.Services.AddScoped<IBirimService, BirimManager>();

builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuManager>();

builder.Services.AddScoped<IMenuRolRepository, MenuRolRepository>();
builder.Services.AddScoped<IMenuRolService, MenuRolManager>();

//eppluss lisansýz kullaným için ISU takma adý kullanýldý. Farklý bir adda kullanýlabilir
ExcelPackage.License.SetNonCommercialPersonal("ISU");

// QuestPDF lisans ayari
QuestPDF.Settings.License = LicenseType.Community;

//ankette link olusturulurken Id deðerini sifrelemek icin kullanilir
builder.Services.AddDataProtection();

//builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Kullanici/Login";
        options.LogoutPath = "/Kullanici/Logout";
        options.AccessDeniedPath = "/Home/Yetkisiz";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(1); //1 dakika gecerlidir
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers().AddFluentValidation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

//sayfa bulunamazsa hata sayfasýna yönlendirir
//app.UseExceptionHandler("/Home/Hata?kod=500");
//app.UseStatusCodePagesWithReExecute("/Home/Hata", "?kod={0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Kullanici}/{action=Login}/{id?}");

app.Run();
