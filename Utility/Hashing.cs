using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Utility
{
    public class Hashing
    {
        public static String hashData(String data, String salt)
        {
            try
            {
                int iterations = 2048;
                int keyLength = 32 * 8; // 32 bytes * 8 bits

                char[] dataChars = data.ToCharArray();
                byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt); ;

                var pbkdf2 = new Rfc2898DeriveBytes(data, saltBytes, iterations, HashAlgorithmName.SHA512);
                return Convert.ToBase64String(pbkdf2.GetBytes(keyLength));
            }
            catch (Exception ex)
            {

            }
            return "";
        }
        public static bool hashVerify(String data,string hash, String salt)
        {
            try
            {
                int iterations = 2048;
                int keyLength = 32 * 8; // 32 bytes * 8 bits

                char[] dataChars = data.ToCharArray();
                byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt); ;

                var pbkdf2 = new Rfc2898DeriveBytes(data, saltBytes, iterations, HashAlgorithmName.SHA512);
                return Convert.ToBase64String(pbkdf2.GetBytes(keyLength)) == hash;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}