using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ISUAnket.WEB.Models.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// enum tanımlamalarında kullanıcıya tanımlanan Display içerisindeki değerleri gösterir
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue
                .GetType()
                .GetMember(enumValue.ToString())[0]
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? enumValue.ToString();
        }
    }
}
