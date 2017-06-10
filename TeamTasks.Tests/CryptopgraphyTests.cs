using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using System;

namespace TeamTasks.Tests
{
    public static class CryptopgraphyTests
    {
        public static void TestCryptography()
        {
            string message = "This is my message";
            //string key = "E546C8DF278CD5931069B522E695D4F3";
            string key = "SKXLZ81FNZn@3451SKXLZ81FNZn@441^";

            Crypt crypt = new Crypt();
            string encrypted = crypt.Encrypt(message, key);

            string decrypted = crypt.Decrypt(encrypted, key);

            var dummy = 3;
        }

        public static void TestWebTokens()
        {
            Crypt crypt = new Crypt();
            string key = "LL132-xzJ41-N11!";

            WebToken token = new WebToken();
            token.CreatedDate = DateTime.Now;
            token.Issuer = "Test Issuer";
            token.AddClaim("username", "admin");
            token.AddClaim("cityofbirth", "Parañaque");

            string encryptedToken = token.GenerateToken(crypt, key);

            WebToken decryptedToken = WebToken.DecryptToken(encryptedToken, crypt, key);

            var dummy = 3;
        }
    }

    
}
