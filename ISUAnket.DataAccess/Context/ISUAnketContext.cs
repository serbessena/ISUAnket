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
    }
}
