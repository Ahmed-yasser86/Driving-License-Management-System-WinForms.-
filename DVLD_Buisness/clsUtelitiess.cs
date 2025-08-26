using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
   public class clsUtelitiess
    {


        public static string HashData(string input)
        {

            using (SHA256 Sh265 = SHA256.Create())
            {

                byte[] Hash = Sh265.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(Hash).Replace("-", "");


            }



        }



    }
}
