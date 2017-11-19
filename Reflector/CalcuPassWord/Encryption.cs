namespace CalcuPassWord
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Encryption
    {
        public static string DesDecrypt(string decryptString, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] rgbIV = bytes;
            byte[] buffer = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static string DesEncrypt(string encryptString, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] rgbIV = bytes;
            byte[] buffer = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        public static string DisEncryPW(string strPass, string Key)
        {
            return DesDecrypt(strPass, Key);
        }

        public static string EncryPW(string Pass, string Key)
        {
            return DesEncrypt(Pass, Key);
        }
    }
}

