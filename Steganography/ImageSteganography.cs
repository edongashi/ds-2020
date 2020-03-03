using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Steganography
{
    static class ImageSteganography
    {
        public static Bitmap Write(Bitmap bitmap, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            int length = data.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);
            byte checksum = data.Aggregate((a, b) => (byte)(a ^ b));
            byte[] checksumBytes = new byte[1] { checksum };

            // <length> <data> <checksum>
            byte[] bytes = Utils.Join(
                lengthBytes,
                data,
                checksumBytes
            );

            return BitmapWriter.Write(bitmap, bytes);
        }

        public static string Read(Bitmap bitmap)
        {
            byte[] lengthBytes = BitmapWriter.Read(bitmap, 0, sizeof(int));
            int length = BitConverter.ToInt32(lengthBytes, 0);
            byte[] data = BitmapWriter.Read(bitmap, sizeof(int), length);
            byte[] checksumBytes = BitmapWriter.Read(bitmap, sizeof(int) + length, 1);
            byte checksum = checksumBytes[0];

            if (checksum != data.Aggregate((a, b) => (byte)(a ^ b)))
            {
                throw new Exception("Checksum error.");
            }

            return Encoding.UTF8.GetString(data);
        }
    }
}
