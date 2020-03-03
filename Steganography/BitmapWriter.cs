using System.Drawing;

namespace Steganography
{
    static class BitmapWriter
    {
        public static Bitmap Write(Bitmap bitmap, byte[] data)
        {
            Bitmap clone = (Bitmap)bitmap.Clone();
            int width = bitmap.Width;
            BitArray bits = new BitArray(data);

            for (int i = 0; i < bits.Length; i++)
            {
                int pixel = i / 3;
                int channel = i % 3;

                int x = pixel % width;
                int y = pixel / width;

                bool bit = bits.GetBit(i);

                int color = clone.GetPixel(x, y).ToArgb();
                int newColor = Utils.SetBit(
                     color,
                     channel * 8,
                     bit
                );

                clone.SetPixel(x, y, Color.FromArgb(newColor));
            }

            return clone;
        }

        public static byte[] Read(Bitmap bitmap, int offset, int length)
        {
            byte[] data = new byte[length];
            int width = bitmap.Width;
            BitArray bits = new BitArray(data);

            for (int i = 0; i < bits.Length; i++)
            {
                int index = offset * 8 + i;
                int pixel = index / 3;
                int channel = index % 3;

                int x = pixel % width;
                int y = pixel / width;

                int color = bitmap.GetPixel(x, y).ToArgb();
                bool bit = Utils.GetBit(color, channel * 8);
                bits.SetBit(i, bit);
            }

            return data;
        }
    }
}
