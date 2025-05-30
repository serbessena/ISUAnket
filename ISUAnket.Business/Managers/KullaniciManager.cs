using ISUAnket.Business.Interfaces;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Managers
{
    public class KullaniciManager : IKullaniciService
    {
        private readonly IKullaniciRepository _kullaniciRepository;

        public KullaniciManager(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<List<Kullanici>> KullanicilaraGoreRolListesiServiceAsync()
        {
            return await _kullaniciRepository.KullanicilaraGoreRolListesiAsync();
        }

        public async Task<List<Kullanici>> GetListAllServiceAsync()
        {
            return await _kullaniciRepository.GetListAllAsync();
        }

        public async Task<List<Kullanici>> GetAllServiceAsync(Expression<Func<Kullanici, bool>> predicate)
        {
            return await _kullaniciRepository.GetAllAsync(predicate);
        }

        public async Task<Kullanici> GetByIdServiceAsync(int id)
        {
            return await _kullaniciRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(Kullanici entity)
        {
            await _kullaniciRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Kullanici entity)
        {
            await _kullaniciRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(Kullanici entity)
        {
            await _kullaniciRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _kullaniciRepository.ChangeActivePasiveStatusAsync(id);
        }

       
        public async Task<bool> RegisterAsync(Kullanici yeniKullanici)
        {
            var mevcut = await _kullaniciRepository.GetAllAsync(x => x.KulaniciAdi == yeniKullanici.KulaniciAdi);
            var tcknKontrol = await _kullaniciRepository.GetAllAsync(x => x.TCKN == yeniKullanici.TCKN);


            if (mevcut.Any() || tcknKontrol.Any())
            {
                return false;
            }
                

            yeniKullanici.Sifre = HashPassword(yeniKullanici.Sifre);

            await _kullaniciRepository.AddAsync(yeniKullanici);

            return true;
        }


        public async Task<Kullanici> LoginAsync(string kullaniciAdi, string sifre)
        {
            var kullanici = await _kullaniciRepository
                                    .GetAllAsync(x => x.KulaniciAdi == kullaniciAdi && x.AktifMi);

            var user = kullanici.FirstOrDefault();
            if (user == null || !VerifyPassword(sifre, user.Sifre))
                return null;

            user.OturumAcikMi = true;

            await _kullaniciRepository.UpdateAsync(user);

            return user;
        }

        // ✅ Şifre Değiştirme
        public async Task<bool> SifreDegistirAsync(int kullaniciId, string eskiSifre, string yeniSifre)
        {
            var user = await _kullaniciRepository.GetByIdAsync(kullaniciId);

            if (user == null || !VerifyPassword(eskiSifre, user.Sifre))
                return false;

            user.Sifre = HashPassword(yeniSifre);

            await _kullaniciRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> LogoutAsync(int kullaniciId)
        {
            var user = await _kullaniciRepository.GetByIdAsync(kullaniciId);

            if (user == null)
                return false;

            
            user.SonCikisTarihi = DateTime.Now; 
            user.OturumAcikMi = false; 
            await _kullaniciRepository.UpdateAsync(user);

            return true;
        }

        public async Task<string> SifremiUnuttumAsync(string kullaniciAdiOrTckn)
        {
            var kullanici = (await _kullaniciRepository.GetAllAsync(x =>
       x.KulaniciAdi == kullaniciAdiOrTckn || x.TCKN == kullaniciAdiOrTckn)).FirstOrDefault();

            if (kullanici == null)
            {
                return "Kullanıcı bulunamadı.";
            }

            // Geçici şifre oluştur
            var geciciSifre = Guid.NewGuid().ToString().Substring(0, 8);

            kullanici.Sifre = HashPassword(geciciSifre);

            await _kullaniciRepository.UpdateAsync(kullanici);

            // E-posta gönderimi
            var konu = "Şifre Sıfırlama";
            var mesaj = $"Merhaba {kullanici.Ad},\n\nGeçici şifreniz: {geciciSifre}\nLütfen sisteme giriş yaptıktan sonra şifrenizi değiştiriniz.\n\nISUAnket Sistemi";

            try
            {
                //await MailGonderAsync(kullanici.Email, konu, mesaj);
            }
            catch (Exception ex)
            {
                return $"Şifre oluşturuldu ancak e-posta gönderilemedi: {ex.Message}";
            }

            return "Geçici şifre e-posta adresinize gönderildi.";
        }

        public async Task<Kullanici?> KullaniciProfilGetirAsync(int kullaniciId)
        {
            return await _kullaniciRepository.GetByIdAsync(kullaniciId);
        }

        public async Task<bool> RolAtaAsync(int kullaniciId, int rolId)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(kullaniciId);

            if (kullanici==null)
            {
                return false;
            }

            kullanici.RolId = rolId;

            await _kullaniciRepository.UpdateAsync(kullanici);

            return true;
        }

        #region Fonksiyon Bölümü


        /// <summary>
        /// şifreyi şifrelenmiş biçimde tutar
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hash = HashPassword(inputPassword);
            return storedHash == hash;
        }

        /// <summary>
        /// Şifremi unuttum sayfası geçici şifre oluşturur
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GenerateTemporaryPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task MailGonderAsync(string aliciEmail, string konu, string mesaj)
        {
            using var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential("youremail@gmail.com", "uygulama-sifresi")
            };

            var mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress("youremail@gmail.com", "ISUAnket");
            mail.To.Add(aliciEmail);
            mail.Subject = konu;
            mail.Body = mesaj;

            await smtp.SendMailAsync(mail);
        }

        



        #endregion




    }
}
