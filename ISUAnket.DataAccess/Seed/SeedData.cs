using ISUAnket.DataAccess.Context;
using ISUAnket.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Seed
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new ISUAnketContext(
                serviceProvider.GetRequiredService<DbContextOptions<ISUAnketContext>>()))
            {
                // Eğer veritabanı yoksa oluştur
                await context.Database.MigrateAsync();

                // Roller Seed
                if (!context.Roller.Any())
                {
                    context.Roller.AddRange(
                        new Rol {  RolAdi = "SüperAdmin", AktifMi = true },
                        new Rol {  RolAdi = "Admin", AktifMi = true }
                    );

                    await context.SaveChangesAsync();
                }

                var superAdminRole = await context.Roller.FirstAsync(r => r.RolAdi == "SüperAdmin");
                var adminRole = await context.Roller.FirstAsync(r => r.RolAdi == "Admin");

                // Kullanıcılar Seed
                if (!context.Kullanicilar.Any())
                {
                    context.Kullanicilar.AddRange(
                        new Kullanici
                        {
                            TCKN = "11111111111",
                            Ad = "Süper",
                            Soyad = "Admin",
                            KulaniciAdi = "superadmin",
                            Email = "superadmin@test.com",
                            Sifre = HashPassword("superadmin123*"),
                            OturumAcikMi = false,
                            AktifMi = true,
                            RolId = superAdminRole.Id // ✅
                        },
                        new Kullanici
                        {
                            TCKN = "22222222222",
                            Ad = "admin",
                            Soyad = "admin",
                            KulaniciAdi = "admin",
                            Email = "admin@test.com",
                            Sifre = HashPassword("admin123*"),
                            OturumAcikMi = false,
                            AktifMi = true,
                            RolId = adminRole.Id // ✅
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
