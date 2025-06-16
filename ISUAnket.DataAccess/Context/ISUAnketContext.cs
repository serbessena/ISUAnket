using ISUAnket.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Context
{
    public class ISUAnketContext : DbContext
    {
        

        public ISUAnketContext(DbContextOptions<ISUAnketContext> options)
            : base(options)
        {
        }

        public DbSet<Anket> Anketler { get; set; }
        public DbSet<Soru> Sorular { get; set; }
        public DbSet<Cevap> Cevaplar{ get; set; }
        public DbSet<Kullanici> Kullanicilar{ get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Birim> Birimler { get; set; }

        //ilk migration oluşturukurken bu alan yorum satırından çıkarılmalıdır
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Rol>().HasData(
        //            new Rol
        //            {
        //                Id = 1,
        //                RolAdi = "SüperAdmin",
        //                AktifMi = true
        //            },
        //            new Rol
        //            {
        //                Id = 2,
        //                RolAdi = "Admin",
        //                AktifMi = true
        //            }
        //        );

        //    modelBuilder.Entity<Kullanici>()
        //        .HasData(
        //            new Kullanici
        //            {
        //                Id = 1,
        //                TCKN = "11111111111",
        //                Ad = "Süper",
        //                Soyad = "Admin",
        //                KulaniciAdi = "superadmin",
        //                Sifre = "aF3p+5z1NcVqpjFD4CqH+NAl8FJv5n9nmb6SYsFHv7M=",
        //                //"superadmin123*"
        //                OturumAcikMi = false,
        //                AktifMi = true,
        //                RolId = 1
        //            },
        //            new Kullanici
        //            {
        //                Id = 2,
        //                TCKN = "11111111111",
        //                Ad = "admin",
        //                Soyad = "admin",
        //                KulaniciAdi = "admin",
        //                Sifre = "Xx7hHGBzPQhM56OxAfWgYvVwjW40E+gZ2KMKMnq/5mU=",
        //                //"admin123*"
        //                OturumAcikMi = false,
        //                AktifMi = true,
        //                RolId = 2
        //            }
        //        );
        //}
    }
}
