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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=BILGIISLEM03;Initial Catalog=ISUAnketDb;User Id=sa;Password=SenaDuru123;TrustServerCertificate=True;");
        }

        public DbSet<Anket> Anketler { get; set; }
        public DbSet<Soru> Sorular { get; set; }
        public DbSet<Cevap> Cevaplar{ get; set; }
        public DbSet<Kullanici> Kullanicilar{ get; set; }
        public DbSet<Rol> Roller { get; set; }
    }
}
