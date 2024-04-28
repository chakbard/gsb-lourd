using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace WebGSB.Models.Utilitaires
{
    public class FonctionsUtiles
    {
        public FonctionsUtiles()
        {
        }

        public static String md5(String input)
        {
            String result = input;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, input);
                return hash;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        /// <summary>
        /// Convertir une chaine date de mysql en datetime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime StringToDate(String dt)
        {
            String[] coupehr = dt.Split(' ');
            String[] coupedt = coupehr[0].Split('/');
            return new DateTime(int.Parse(coupedt[2]), int.Parse(coupedt[1]), int.Parse(coupedt[0]));
        }

        /// <summary>
        /// Convertir une datetime en chaine pour mysql
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static String DateToString(DateTime dt)
        {
            String retour = "";
            retour += dt.Year + "-";
            retour += dt.Month + "-";
            retour += dt.Day + " ";
            retour += dt.Hour + ":";
            retour += dt.Minute + ":";
            retour += dt.Second;
            return retour;
        }
    }
}