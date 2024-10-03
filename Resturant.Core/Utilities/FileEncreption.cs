using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Resturant.Core.Utilities
{
    public static class FileEncreption
    {
        public static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                var password = @"myKey123"; // Your Key Here
                var ue = new UnicodeEncoding();
                var key = ue.GetBytes(password);

                var cryptFile = outputFile;
                var fsCrypt = new FileStream(cryptFile, FileMode.Create);

                var rmCrypto = new RijndaelManaged();

                var cs = new CryptoStream(fsCrypt,
                    rmCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                var fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                // ignored
            }
        }

        public static void DecryptFile(string inputFile, string outputFile)
        {

            {
                var password = @"myKey123"; // Your Key Here
                var ue = new UnicodeEncoding();
                var key = ue.GetBytes(password);

                var fsCrypt = new FileStream(inputFile, FileMode.Open);

                var rmCrypto = new RijndaelManaged();

                var cs = new CryptoStream(fsCrypt,
                    rmCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                var fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }


        public static byte[] DecryptFile(string inputFile)
        {
            var password = @"myKey123"; // Your Key Here
            var ue = new UnicodeEncoding();
            var key = ue.GetBytes(password);

            var fsCrypt = new FileStream(inputFile, FileMode.Open);

            var rmCrypto = new RijndaelManaged();
            var cs = new CryptoStream(fsCrypt,
                    rmCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);
            var fsOut = new MemoryStream();
            var data = 0;
            while (Assign(ref data, cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

            return fsOut.ToArray();
        }

        private static T Assign<T>(ref T source, T value)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            source = value;
            return value;
        }
    }
}
