using FluentValidation.AspNetCore;
using ISUAnket.Business.Interfaces;
using ISUAnket.Business.Managers;
using ISUAnket.DataAccess.Context;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ISUAnketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddSession();

builder.Services.AddControllers().AddFluentValidation();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
