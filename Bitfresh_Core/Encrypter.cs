using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Bitfresh_Core
{
    public static class Encrypter
    {
        public static byte[] Decrypt(Rijndael encryption, SecureString key, byte[] data)
        {
            IntPtr marshalledKeyBytes = Marshal.SecureStringToGlobalAllocAnsi(key);
            byte[] keyBytes = new byte[encryption.KeySize / 8];
            byte[] decryptedData = new byte[data.Length];

            Marshal.Copy(marshalledKeyBytes, keyBytes, 0, Math.Min(keyBytes.Length, key.Length));

            encryption.Key = keyBytes;

            MemoryStream memoryStream = new MemoryStream(data, 0, data.Length);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryption.CreateDecryptor(), CryptoStreamMode.Read);

            cryptoStream.Read(decryptedData, 0, decryptedData.Length);
            cryptoStream.Close();
            memoryStream.Close();

            for (int i = 0; i < keyBytes.Length; i++)
                keyBytes[i] = 0;

            Array.Clear(encryption.Key, 0, encryption.Key.Length);

            Marshal.ZeroFreeGlobalAllocAnsi(marshalledKeyBytes);

            return decryptedData;
        }

        public static byte[] Encrypt(Rijndael encryption, SecureString key, byte[] data)
        {
            IntPtr marshalledKeyBytes = Marshal.SecureStringToGlobalAllocAnsi(key);
            byte[] keyBytes = new byte[encryption.KeySize / 8];

            Marshal.Copy(marshalledKeyBytes, keyBytes, 0, Math.Min(keyBytes.Length, key.Length));

            encryption.Key = keyBytes;

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryption.CreateEncryptor(), CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            memoryStream.Close();

            for (int i = 0; i < keyBytes.Length; i++)
                keyBytes[i] = 0;

            Array.Clear(encryption.Key, 0, encryption.Key.Length);

            Marshal.ZeroFreeGlobalAllocAnsi(marshalledKeyBytes);

            return memoryStream.ToArray();
        }

        public static byte[] Encrypt(Rijndael encryption, SecureString key, string data)
        {
            return Encrypt(encryption, key, Encoding.ASCII.GetBytes(data));
        }

        public static byte[] Encrypt(Rijndael encryption, SecureString key, SecureString data)
        {
            IntPtr marshalledDataBytes = Marshal.SecureStringToGlobalAllocAnsi(data);
            byte[] dataBytes = new byte[data.Length];

            Marshal.Copy(marshalledDataBytes, dataBytes, 0, dataBytes.Length);

            byte[] encryptedData = Encrypt(encryption, key, dataBytes);

            for (int i = 0; i < dataBytes.Length; i++)
                dataBytes[i] = 0;

            Marshal.ZeroFreeGlobalAllocAnsi(marshalledDataBytes);

            return encryptedData;
        }

        public static String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static SecureString StringtoSecureString(String unsecure)
        {
            SecureString secure = new SecureString();
            foreach (char c in unsecure)
            {
                secure.AppendChar(c);
            }

            return secure;
        }
    }
}