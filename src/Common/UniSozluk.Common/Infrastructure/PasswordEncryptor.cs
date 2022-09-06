using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UniSozluk.Common.Infrastructure
{
    public class PasswordEncryptor
    {
        public static string Encryt(string password)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes=Encoding.ASCII.GetBytes(password);
            byte[] hashBytes=md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
