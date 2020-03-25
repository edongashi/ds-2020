using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rsa
{
    class Program
    {
        static void Main(string[] args)
        {
            var alice = CreateAlice();
            var bob = CreateBob();

            // A -> B
            Console.WriteLine("A -> B");

            // A:encrypt(public_B)
            string encryptedMessageFromAliceToBob
               = alice.EncryptTextFor("Hello", bob.GetPublicKey());

            // A:send
            Console.WriteLine($"Alice has sent to Bob: {encryptedMessageFromAliceToBob}");

            // B:decrypt(private_B)
            string decryptedMessageFromAliceToBob
                = bob.DecryptText(encryptedMessageFromAliceToBob);

            Console.WriteLine($"Bob has received from Alice: {decryptedMessageFromAliceToBob}");

            Console.WriteLine();

            // B -> A
            Console.WriteLine("B -> A");

            // B:encrypt(public_A)
            string encryptedMessageFromBobToAlice
                = bob.EncryptTextFor("hello back", alice.GetPublicKey());

            // B:send
            Console.WriteLine($"Bob has sent to Alice: {encryptedMessageFromBobToAlice}");

            // A:decrypt(private_A)
            string decryptedMessageFromBobToAlice
                = alice.DecryptText(encryptedMessageFromBobToAlice);

            Console.WriteLine($"Alice has received from Bob: {decryptedMessageFromBobToAlice}");

            Console.WriteLine();
        }

        static RsaEncryptor CreateAlice()
        {
            // string alicePublicKey = File.ReadAllText("public_alice.xml");
            string alicePrivateKey = File.ReadAllText("private_alice.xml");
            RsaEncryptor alice = new RsaEncryptor(alicePrivateKey);
            return alice;
        }

        static RsaEncryptor CreateBob()
        {
            // string bobPublicKey = File.ReadAllText("public_bob.xml");
            string bobPrivateKey = File.ReadAllText("private_bob.xml");
            RsaEncryptor bob = new RsaEncryptor(bobPrivateKey);
            return bob;
        }

        static void RsaTest()
        {
            RsaEncryptor rsa = new RsaEncryptor();

            string plaintext = "hello";
            Console.WriteLine($"Plaintext: {plaintext}");

            string ciphertext = rsa.EncryptText(plaintext);
            Console.WriteLine($"Ciphertext: {ciphertext}");

            string decrypted = rsa.DecryptText(ciphertext);
            Console.WriteLine($"Decrypted plaintext: {decrypted}");

            File.WriteAllText("public.xml", rsa.GetPublicKey());
            File.WriteAllText("private.xml", rsa.GetPrivateKey());
        }
    }

    class RsaEncryptor
    {
        private readonly RSACryptoServiceProvider rsacsp;

        public RsaEncryptor()
        {
            rsacsp = new RSACryptoServiceProvider();
        }

        public RsaEncryptor(string privateKey)
        {
            rsacsp = new RSACryptoServiceProvider();
            rsacsp.FromXmlString(privateKey);
        }

        public string GetPublicKey()
        {
            return rsacsp.ToXmlString(false);
        }

        public string GetPrivateKey()
        {
            return rsacsp.ToXmlString(true);
        }

        public string EncryptTextFor(string plaintext, string publicKey)
        {
            var otherParty = new RSACryptoServiceProvider();
            otherParty.FromXmlString(publicKey);

            return EncryptText(plaintext, otherParty);
        }

        public string EncryptText(string plaintext)
        {
            return EncryptText(plaintext, this.rsacsp);
        }

        public string DecryptText(string ciphertext)
        {
            // Decryption

            // ciphertext (base64) -> ciphertext (byte[]) - Convert.FromBase64String
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            // ciphertext (byte[]) -> plaintext (byte[])  - rsa.Decrypt
            byte[] plaintextBytes = rsacsp.Decrypt(ciphertextBytes, true);

            // plaintext (byte[])  -> plaintext (string)  - Encoding.UTF8.GetString
            string plaintext = Encoding.UTF8.GetString(plaintextBytes);

            return plaintext;
        }

        private static string EncryptText(string plaintext, RSACryptoServiceProvider rsa)
        {
            // Encryption

            // plaintext (string)  -> plaintext (byte[])  - Encoding.UTF8.GetBytes
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            // plaintext (byte[])  -> ciphertext (byte[]) - rsa.Encrypt
            byte[] ciphertextBytes = rsa.Encrypt(plaintextBytes, true);

            // ciphertext (byte[]) -> ciphertext (base64) - Convert.ToBase64String
            string ciphertextString = Convert.ToBase64String(ciphertextBytes);

            return ciphertextString;
        }
    }
}
