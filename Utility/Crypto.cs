using Rimit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Utility
{
    public class Crypto
    {
        protected RijndaelManaged myRijndael;

        public string encryptRimitData(string plaintext, string password)
        {
            string iv = GenerateKeyAndIV();
            using (myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = HexStringToByte(password);
                myRijndael.IV = HexStringToByte(iv);
                myRijndael.Mode = CipherMode.CBC;
                myRijndael.Padding = PaddingMode.PKCS7;
                byte[] encrypted = EncryptStringToBytes(plaintext, myRijndael.Key, myRijndael.IV);
                byte[] data = myRijndael.IV.Concat(encrypted).ToArray();
                string encString = Convert.ToBase64String(data);
                string salt = iv + iv;

                var hash = Hashing.hashData(encString,salt);
                EncryptedData response = new EncryptedData();
                response.iv = iv;
                response.cipher_text = encString;
                response.hash = hash;
                var json = new JavaScriptSerializer().Serialize(response);

                return json;
;
            }
           // return "";
        }

        public string decryptRimitData(EncryptedData encryptedString, string encryptionKey)
        {
            string iv = encryptedString.iv;
            using (myRijndael = new RijndaelManaged())

            {
                myRijndael.Key = HexStringToByte(encryptionKey);
                myRijndael.Mode = CipherMode.CBC;
                myRijndael.Padding = PaddingMode.PKCS7;
                Byte[] ourEnc = Convert.FromBase64String(encryptedString.cipher_text);
                byte[] enc = Convert.FromBase64String(encryptedString.cipher_text);
                myRijndael.IV = enc.Take(16).ToArray();
                enc = enc.Skip(16).ToArray();
                string ourDec = DecryptStringFromBytes(enc, myRijndael.Key, myRijndael.IV);
                string salt = iv + iv;
                var hash = Hashing.hashVerify(encryptedString.cipher_text,encryptedString.hash, salt);
                if (!hash)
                {
                    return "";
                }
                return ourDec;
            }
        }
        protected string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)

                throw new ArgumentNullException("cipherText");

            if (Key == null || Key.Length <= 0)

                throw new ArgumentNullException("Key");

            if (IV == null || IV.Length <= 0)

                throw new ArgumentNullException("Key");
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;

                rijAlg.IV = IV;
                // Create a decrytor to perform the stream transform.

                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for decryption.

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))

                {

                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))

                    {

                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                        {
                            plaintext = srDecrypt.ReadToEnd();

                        }

                    }

                }
            }
            return plaintext;
        }
        public string GenerateKeyAndIV()
        {
            // This code is only here for an example
            RijndaelManaged myRijndaelManaged = new RijndaelManaged();
            myRijndaelManaged.Mode = CipherMode.CBC;
            myRijndaelManaged.Padding = PaddingMode.PKCS7;
            myRijndaelManaged.GenerateIV();
            myRijndaelManaged.GenerateKey();
            string newinitVector = ByteArrayToHexString(myRijndaelManaged.IV);
            return newinitVector;
        }
        protected static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }
        protected byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.

            if (plainText == null || plainText.Length <= 0)

                throw new ArgumentNullException("plainText");

            if (Key == null || Key.Length <= 0)

                throw new ArgumentNullException("Key");

            if (IV == null || IV.Length <= 0)

                throw new ArgumentNullException("Key");

            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                // Create a decrytor to perform the stream transform. 
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream. 
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public string decryptRimitData(string ivHashCiphertext, string password)
        {
            return "";
        }
    }
}