using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Resturant.Core
{
    public static class StringEncryption
    {
        public static string Decrypt(string textToBeDecrypted, string key)
        {
            var rijndaelCipher = new RijndaelManaged();

            string decryptedData;

            try
            {
                var encryptedData = Convert.FromBase64String(textToBeDecrypted);

                var salt = Encoding.ASCII.GetBytes(key.Length.ToString());
                //Making of the key for decryption
                var secretKey = new PasswordDeriveBytes(key, salt);
                //Creates a symmetric Rijndael decryptor object.
                var decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));

                var memoryStream = new MemoryStream(encryptedData);
                //Defines the cryptographics stream for decryption.THe stream contains decrpted data
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                var plainText = new byte[encryptedData.Length];
                var decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                memoryStream.Close();
                cryptoStream.Close();

                //Converting to string
                decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
            }
            catch
            {
                decryptedData = textToBeDecrypted;
            }
            return decryptedData;
        }

        public static string Encrypt(string textToBeEncrypted, string key)
        {
            var rijndaelCipher = new RijndaelManaged();
            var plainText = Encoding.Unicode.GetBytes(textToBeEncrypted);
            var salt = Encoding.ASCII.GetBytes(key.Length.ToString());
            var secretKey = new PasswordDeriveBytes(key, salt);
            //Creates a symmetric encryptor object. 
            var encryption = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            var memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            var cryptoStream = new CryptoStream(memoryStream, encryption, CryptoStreamMode.Write);
            cryptoStream.Write(plainText, 0, plainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            var cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            var encryptedData = Convert.ToBase64String(cipherBytes);

            return encryptedData;
        }
    }
}