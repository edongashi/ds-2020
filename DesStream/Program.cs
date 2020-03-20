using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DesStream
{
    class Program
    {
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("12345678");

        static void Main(string[] args)
        {
            // des encrypt <input-file> <out-file>
            // des encrypt
            // des decrypt <input-file> <out-file>
            // des decrypt

            var key = Encoding.UTF8.GetBytes("fiek2018");
            var command = args.FirstOrDefault();

            Stream OpenInput()
            {
                return args.Length >= 2
                    ? File.OpenRead(args[1])
                    : Console.OpenStandardInput();
            }

            Stream OpenOutput()
            {
                return args.Length >= 3
                    ? File.OpenWrite(args[2])
                    : Console.OpenStandardOutput();
            }

            switch (command)
            {
                case "encrypt":
                    Encrypt(key, OpenInput(), OpenOutput());
                    break;

                case "decrypt":
                    Decrypt(key, OpenInput(), OpenOutput());
                    break;

                default:
                    Usage();
                    break;
            }
        }

        private static void Usage()
        {
            Console.WriteLine("Argumente jo valide.");
        }

        private static void Encrypt(byte[] key, Stream inputStream, Stream outputStream)
        {
            using (var des = new DESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros,
                IV = iv
            })
            using (var encryptor = des.CreateEncryptor())
            using (var cryptoStream = new CryptoStream(
                outputStream,
                encryptor,
                CryptoStreamMode.Write)
            )
            {
                inputStream.CopyTo(cryptoStream);
            }
        }

        private static void Decrypt(byte[] key, Stream inputStream, Stream outputStream)
        {
            using (var des = new DESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros,
                IV = iv
            })
            using (var decryptor = des.CreateDecryptor())
            using (var cryptoStream = new CryptoStream(
                inputStream,
                decryptor,
                CryptoStreamMode.Read)
            )
            {
                cryptoStream.CopyTo(outputStream);
            }
        }
    }
}
