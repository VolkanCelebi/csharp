//c# çoklu değer döndürme yöntemleri
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonkDegerDondur
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Çoklu değer döndürme yöntemleri:");
            Console.WriteLine("-----------Listeden Dönen--------------------");

            List<string> elemanlar = listeDonder();
            foreach (var item in elemanlar)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("----------Sözlükten Dönen--------------------");

            Dictionary<int, string> sozlukElemanlari = sozlukdenDonder();
            foreach (var item in sozlukElemanlari)
            {
                Console.WriteLine(item.Key + ". sıra: " + item.Value);
            }

            Console.WriteLine("----------Değer çiftinden dönen-----------------");
            KeyValuePair<string, string> degerCifti = degerdenDonen();
            Console.WriteLine($"{degerCifti.Key} : {degerCifti.Value}");

            Console.WriteLine("----------Class dan dönen-----------------");
            var siniftan = siniftanDonder();
            Console.WriteLine($"{siniftan.deger1}, {siniftan.deger2}, {siniftan.deger3}");

            Console.WriteLine("----------Tuple dönen-----------------");
            var tuple1 = tupleDon();
            Console.WriteLine($"{tuple1.Item1}, {tuple1.Item2}, {tuple1.Item3}");

            //tuple 2. yol
            var (deg1, deg2, deg3) = tupleDon2();
            Console.WriteLine($"{deg1},{deg2},{deg3}");

            //tuple 3. yol
            var tumElemanlar = tupleDon3();
            Console.WriteLine($"{tumElemanlar.el1},{tumElemanlar.el2},{tumElemanlar.el3}");

            Console.ReadLine();

        }

        public static List<string> listeDonder()
        {
            List<string> listem = new List<string>(
                new string[] { 
                "eleman1", "eleman2","eleman3"
                }
                );
            return listem;
        }

        public static Dictionary<int,string> sozlukdenDonder()
        {
            Dictionary<int, string> sozluk = new Dictionary<int, string>()
            {
                {1,"eleman1" },
                {2,"eleman2" },
                {3,"eleman3" }
            };

            return sozluk;
        }

        public static KeyValuePair<string,string> degerdenDonen()
        {
            return new KeyValuePair<string, string>("1","eleman1");
        }

        public static SinifDonder siniftanDonder()
        {
            return new SinifDonder { 
                deger1 = "eleman1", 
                deger2="eleman2", 
                deger3 = "eleman3"
            };
        }

        public static Tuple<string,string,string> tupleDon()
        {
            return new Tuple<string, string, string>("eleman1", "eleman2", "eleman3");
        }

        public static (string,string,string) tupleDon2()
        {
            return ("eleman1", "eleman2", "eleman3");
        }

        public static (string el1, string el2, string el3) tupleDon3()
        {
            return ("eleman1", "eleman2", "eleman3");
        }
        //--
    }

    public class SinifDonder
    {
        public string deger1 { get; set; }
        public string deger2 { get; set; }
        public string deger3 { get; set; }
    }

  
}
