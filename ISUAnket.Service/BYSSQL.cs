using ISUAnket.Service.ViewModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Service
{
    public class BYSSQL
    {
        private static string connectionstring = "Server=172.16.7.80;Username=bys;Database=element;Port=5000;Password=ByS+1_2023!;SSLMode=Disable";

        public static dynamic SqlSorgu(string GorevKod = null, int MerkezTasra = 0)
        {
            return PersonelGetir(MerkezTasra, GorevKod);
        }

        private static List<PersonelSayilariView> PersonelGetir(int MerkezTasra, string GorevKod)
        {
            var SQL = "SELECT " +
                "\"TCKN\", " +
                "\"ADI\", " +
                "\"SOYADI\", " +
                "\"TELEFON\", " +
                "\"MERKEZ_TASRA\", " +
                "\"GOREV_KOD\" " +
                "FROM ( " +
                "SELECT " +
                "\"GEN_SICIL\".\"TKN_VKN\" as \"TCKN\", " +
                "\"GEN_SICIL\".\"ADI\" as \"ADI\", " +
                "\"GEN_SICIL\".\"SOYADI\" as \"SOYADI\", " +
                "replace(\"GOREV_YERI_KURUMSAL\".\"STRING_KOD\",'.00','') as \"GOREV_KOD\", " +
                "\"GOREV_YERI_KURUMSAL\".\"MERKEZ_TASRA\", " +
                "\"GEN_ADRES\".\"CEP_TELEFON_NO\" as \"TELEFON\" " +
                "FROM \"BYS\".\"GEN_SICIL\" " +
                "INNER JOIN \"BYS\".\"GEN_ADRES\" ON \"GEN_ADRES\".\"SICILI_ID\" = \"GEN_SICIL\".\"OID\" " +
                "INNER JOIN \"BYS\".\"PER_KADRO_ATAMA\" ON \"PER_KADRO_ATAMA\".\"SICIL_ID\" = \"GEN_SICIL\".\"OID\" " +
                "INNER JOIN \"BYS\".\"PER_KADRO_TANIM\" ON \"PER_KADRO_ATAMA\".\"KADRO_TANIM_ID\" = \"PER_KADRO_TANIM\".\"OID\" " +
                "INNER JOIN \"BYS\".\"PER_KADRO_KUTUK\" ON \"PER_KADRO_TANIM\".\"KADRO_KUTUK_ID\" = \"PER_KADRO_KUTUK\".\"OID\" " +
                "INNER JOIN \"BYS\".\"PER_KADRO_TIP\" ON \"PER_KADRO_KUTUK\".\"KADRO_TIP_ID\" = \"PER_KADRO_TIP\".\"OID\" " +
                "INNER JOIN \"BYS\".\"PER_GOREV_YERI_TANIM\" ON \"PER_KADRO_ATAMA\".\"GOREV_YERI_TANIM_ID\" = \"PER_GOREV_YERI_TANIM\".\"OID\" " +
                "INNER JOIN \"BYS\".\"GEN_KURUMSAL\" as \"GOREV_YERI_KURUMSAL\" ON \"PER_GOREV_YERI_TANIM\".\"KURUMSAL_ID\" = \"GOREV_YERI_KURUMSAL\".\"OID\" " +
                "INNER JOIN \"BYS\".\"PER_KADRO_KUTUK\" as \"GOREV_KUTUK\" ON \"PER_GOREV_YERI_TANIM\".\"KADRO_KUTUK_ID\" = \"GOREV_KUTUK\".\"OID\" " +
                "WHERE \"PER_KADRO_ATAMA\".\"AKTIF_PASIF\" = 2 " +
                "AND \"PER_KADRO_TIP\".\"OID\" not in (21, 38) " +
                ") AS \"A\" " +
                "WHERE \"A\".\"MERKEZ_TASRA\" = " + MerkezTasra + " " +
                "AND \"A\".\"GOREV_KOD\" LIKE '" + GorevKod + "%' " +
                "ORDER BY " +
                "\"A\".\"ADI\", " +
                "\"A\".\"SOYADI\"";

            List<PersonelSayilariView> personeller = new List<PersonelSayilariView>();
            using (var conn = new NpgsqlConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(SQL, conn))
                {
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                        try
                        {
                            while (reader.Read())
                            {
                                var personel = new PersonelSayilariView
                                {
                                    TCKN = reader.GetString(0),
                                    Cep = reader.IsDBNull(3) ? "" : reader.GetString(3).Length == 10 ? reader.GetString(3) : reader.GetString(3).Substring(1, 10),
                                };
                                personeller.Add(personel);
                            }
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                    else //return msg
                        reader.Close();
                }
                conn.Dispose();
            }
            return personeller;
        }

    }
}
