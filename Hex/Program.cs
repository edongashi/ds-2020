using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hex
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = args.FirstOrDefault();
            if (arg == "encode" || arg == null)
            {
                var inputBytes = ReadToEnd(Console.OpenStandardInput());
                Console.WriteLine(ByteArrayToString(inputBytes));
            }
            else if (arg == "decode")
            {
                var hexString = Console.In.ReadToEnd();
                var outputBytes = StringToByteArray(hexString);
                var outputStream = Console.OpenStandardOutput();
                outputStream.Write(outputBytes, 0, outputBytes.Length);
                outputStream.Flush();
                outputStream.Close();
            }
            else
            {
                Console.WriteLine("Invalid args.");
            }

        }
        public static byte[] StringToByteArray(string hex)
        {
            hex = Regex.Replace(hex, @"\s", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
