namespace ISUAnket.WEB.Helpers
{
    public static class TCKNHelper
    {
        /// <summary>
        /// tc kimlik numarasını maskeli bir şekilde gösterir
        /// </summary>
        /// <param name="tckn"></param>
        /// <returns></returns>
        public static string MaskeliTCKN(string tckn)
        {
            if (string.IsNullOrEmpty(tckn) || tckn.Length<11)
            {
                return tckn;
            }

            // İlk 3 karakter açık, gerisini * yap, son 2 karakter açık
            return tckn.Substring(0, 3) + new string('*', tckn.Length - 5) + tckn.Substring(tckn.Length-2,2);
        }
    }
}
