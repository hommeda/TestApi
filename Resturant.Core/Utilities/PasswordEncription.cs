namespace Resturant.Core.Utilities
{
    public class PasswordEncryption
    {
        private static readonly string Key = "Fady_1&Milad_2&Abo-Zeid_3&Sief_4+AYA_5";

        public static string Decrypt(string textToBeDecrypted)
        {
            return StringEncryption.Decrypt(textToBeDecrypted, Key);
        }

        public static string Encrypt(string textToBeEncrypted)
        {
            return StringEncryption.Encrypt(textToBeEncrypted, Key);
        }
    }
}