using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface IKullaniciService:IGenericService<Kullanici>
    {
        /// <summary>
        /// Yeni kullanıcı kaydı yapar. Şifre hash'lenerek kaydedilir.
        /// </summary>
        /// <param name="kullanici">Kullanıcı bilgileri</param>
        /// <returns>Başarılıysa true</returns>
        Task<bool> RegisterAsync(Kullanici kullanici);

        /// <summary>
        /// Kullanıcı adı ve şifre ile giriş yapılmasını sağlar.
        /// </summary>
        /// <param name="kullaniciAdi">Kullanıcı adı</param>
        /// <param name="sifre">Şifre</param>
        /// <returns>Giriş başarılıysa kullanıcı bilgisi döner, aksi halde null</returns>
        Task<Kullanici?> LoginAsync(string kullaniciAdi, string sifre);

        /// <summary>
        /// Çıkış işlemi. Örneğin kullanıcı token silme gibi işlemler burada yapılabilir.
        /// </summary>
        /// <param name="kullaniciId">Kullanıcı ID</param>
        /// <returns>Başarılıysa true</returns>
        Task<bool> LogoutAsync(int kullaniciId); 

        /// <summary>
        /// Şifremi unuttum işlemi (reset işlemi başlatılır).
        /// </summary>
        /// <param name="kullaniciAdiOrTckn">Kullanıcı adı veya TCKN</param>
        /// <returns>Yeni geçici şifre ya da mesaj</returns>
        Task<string> SifremiUnuttumAsync(string kullaniciAdiOrTckn);

        /// <summary>
        /// Şifreyi değiştirir.
        /// </summary>
        /// <param name="kullaniciId">Kullanıcı ID</param>
        /// <param name="eskiSifre">Mevcut şifre</param>
        /// <param name="yeniSifre">Yeni şifre</param>
        /// <returns>Başarılıysa true</returns>
        Task<bool> SifreDegistirAsync(int kullaniciId, string eskiSifre, string yeniSifre);

        /// <summary>
        /// Kullanıcının profil bilgilerini getirir.
        /// </summary>
        /// <param name="kullaniciId">Kullanıcı ID</param>
        /// <returns>Kullanıcı bilgileri</returns>
        Task<Kullanici?> KullaniciProfilGetirAsync(int kullaniciId);

        /// <summary>
        /// kullanıcıya rol atar
        /// </summary>
        /// <param name="kullaniciId"></param>
        /// <param name="rolId"></param>
        /// <returns></returns>
        Task<bool> RolAtaAsync(int kullaniciId, int rolId);

        /// <summary>
        /// kullanıcları ve bağlı olduğu rolleri liste şeklinde getirir
        /// </summary>
        /// <returns></returns>
        Task<List<Kullanici>> KullanicilaraGoreRolListesiServiceAsync();
    }
}
